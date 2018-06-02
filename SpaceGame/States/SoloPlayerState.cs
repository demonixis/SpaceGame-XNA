using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna.Engine;
using Yna.Engine.Graphics;
using SpaceGame.Manager;
using SpaceGame.UI;
using SpaceGame.UI.Message;
using SpaceGame.Player;
using SpaceGame.Level;
using SpaceGame.Level.Scrolling;
using SpaceGame.Ennemies;
using SpaceGame.States.Menu;
using SpaceGame.Data.Description;

namespace SpaceGame.States
{
    public enum GameStatus
    {
        Starting = 0,
        Running,
        Paused,
        Success,
        Loose,
        Died,
        Quit
    }

    public class SoloPlayerState : YnState2D
    {
        private GameStatus gameStatus;
        private LevelType _levelType;

        private GameUI gameUI;

        // Vaisseau
        private SpacePlayer _player;

        // Enemies
        private EnnemyManager _ennemyManager;

        // Bonus
        private YnGroup bonusGroup;

        // FX
        private FxManager fxManager;

        // Gestionnaire de popup
        private MessageBoxManager messageBoxManager;

        // Fond du jeu
        private BaseScrolling background;

        // Début du jeu
        private YnTimer _startGameTimer;

        // Fin du jeu ?
        private YnTimer _looseTimer;

        public SoloPlayerState(SpaceShipType type, LevelType levelType)
            : base("solostate", true)
        {
            _levelType = levelType;

            if (_levelType == LevelType.Interior)
                background = new InteriorBackground();
            else
                background = new SpaceBackground();

            Add(background);

            // Les bonus sont en dessous de tout
            bonusGroup = new YnGroup();
            Add(bonusGroup);

            // Joueur
            _player = new SpacePlayer(PlayerIndex.One, Registry.SpaceShipDescriptions[(int)type]);
            _player.Viewport = background.PlayableViewport;
            _player.Killed += new EventHandler<EventArgs>(_player_Killed);
            _player.PlayerReallyDead += new EventHandler<EventArgs>(_player_PlayerReallyDead);
            Add(_player);

            Registry.Players = new SpacePlayer[] { _player };
            Registry.ScoreManager.CurrentGameScore = _player.GameScore;

            // Gestionnaire d'ennemies
            _ennemyManager = new EnnemyManager();
            _ennemyManager.EnnemyDied += new EventHandler<EnnemyDeadEventArgs>(alienManager_EnnemyDied);
            Add(_ennemyManager);

            // Interface
            gameUI = new GameUI(Registry.Players);
            Add(gameUI);

            // Gestionnaire de message
            messageBoxManager = new MessageBoxManager();
            Add(messageBoxManager);

            messageBoxManager.MessageBoxStart.CloseRequested += messageBoxStart_CloseRequested;
            messageBoxManager.MessageBoxPause.CloseRequested += messageBoxPause_CloseRequested;
            messageBoxManager.MessageBoxEnd.CloseRequested += messageBoxEnd_CloseRequested;

            // Timers
            _startGameTimer = new YnTimer(2500, 0);
            _startGameTimer.Completed += startGameTimer_Completed;
            _startGameTimer.Start();

            _looseTimer = new YnTimer(5000, 0);
            _looseTimer.Completed += endTimer_Completed;

            // FX
            fxManager = new FxManager();
            Add(fxManager);

            // audio
            Registry.AudioManager.VocalRate = 0;

            // Registry

            Registry.GameStatus = gameStatus;
            Registry.ActiveState = this;
            Registry.Ennemies = _ennemyManager;
            Registry.GameUI = gameUI;
            Registry.Background = background;
            Registry.FX = fxManager;

            ChangeGameStatus(GameStatus.Starting);
        }

        #region Gestion des events

        // Joueur
        void _player_Killed(object sender, EventArgs e)
        {
            ChangeGameStatus(GameStatus.Loose);
        }

        void _player_PlayerReallyDead(object sender, EventArgs e)
        {
            ChangeGameStatus(GameStatus.Died);
        }


        void alienManager_EnnemyDied(object sender, EnnemyDeadEventArgs e)
        {
            Ennemy ennemy = e.Ennemy;

            Random rand = new Random(DateTime.Now.Millisecond);

            BonusType bonusType = BonusType.Debris;
            if (rand.Next(1, 100) > 95)
                bonusType = BonusType.Ammo;

            BaseBonus bonus = new BaseBonus(bonusType, (int)ennemy.X, (int)ennemy.Y);
            bonusGroup.LoadContent();
            bonusGroup.Add(bonus);
        }

        //  Message de début terminé
        void startGameTimer_Completed()
        {
            ChangeGameStatus(GameStatus.Running, true);
        }

        void messageBoxPause_CloseRequested(object sender, EventArgs e)
        {
            ChangeGameStatus(GameStatus.Running, false);
        }

        void messageBoxStart_CloseRequested(object sender, EventArgs e)
        {
            ChangeGameStatus(GameStatus.Running, true);
        }

        void messageBoxEnd_CloseRequested(object sender, EventArgs e)
        {
            _looseTimer.Stop();
            ChangeGameStatus(GameStatus.Running, true);
        }

        void endTimer_Completed()
        {
            ChangeGameStatus(GameStatus.Running, true);
        }

        #endregion

        private void RestartScene()
        {
            // Interface
            gameUI.Revive();

            Registry.AudioManager.MusicVolume = 1.0f;
            if (_levelType == LevelType.Interior)
                Registry.AudioManager.PlayMusic("Audio/DST-ConFuze", true);
            else
                Registry.AudioManager.PlayMusic("Audio/DST-DXT", true);

            // Joueurs
            _player.Revive();

            // Aliens
            _ennemyManager.Revive();

            // Score
            Registry.Players[0].GameScore.Score = 0;

            // Bonus
            bonusGroup.Clear();

            // MessageBox
            messageBoxManager.DesactiveMessageBoxes();

            _looseTimer.Restart();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            #region MessageBox Début de la partie

            if (gameStatus == GameStatus.Starting)
            {
                _startGameTimer.Update(gameTime);
            }

            #endregion

            #region Mise à jour globale du jeu

            else if (gameStatus == GameStatus.Running)
            {
                // Quitter le jeu
                // Todo retour au menu ou affichage du menu de pause
                if (YnG.Keys.Escape)
                {
                    ChangeGameStatus(GameStatus.Paused);
                    return;
                }
            }

            #endregion

            #region MessageBox Partie en pause

            else if (gameStatus == GameStatus.Paused)
            {
                if (YnG.Keys.JustPressed(Keys.Enter))
                    ChangeGameStatus(GameStatus.Quit);
                else if (YnG.Keys.JustPressed(Keys.P))
                    ChangeGameStatus(GameStatus.Running);
            }

            #endregion

            #region MessageBox Fin de partie

            // Si la partie est terminée on réinitialise le tout
            // Et on affichage la box de fin
            else if (gameStatus == GameStatus.Loose)
            {
                _looseTimer.Update(gameTime);

                // Mise à jour de la box de fin
                string message = messageBoxManager.MessageBoxEnd.BaseMessage;
                message += String.Format("\n\nGet ready in {0}\n\n\nOr press Enter or Space to continue", _looseTimer.TimeRemaining);
                messageBoxManager.MessageBoxEnd.Message = message;
            }

            else if (gameStatus == GameStatus.Success)
            {
                YnG.SwitchState(new ScoreState());
            }

            #endregion
        }

        public void ChangeGameStatus(GameStatus newStatus, bool restart = false)
        {
            if (newStatus == GameStatus.Loose)
            {
                // Audio
                Registry.AudioManager.MusicVolume = 0.2f;

                Registry.AudioManager.SpeakAsync("Mission failed ! You're ship has been destroyed !");

                // Interface
                gameUI.Kill();

                // Aliens
                _ennemyManager.Kill();

                // Joueurs
                if (_player.Active)
                    _player.Kill();

                // Messages
                _looseTimer.Start();
                messageBoxManager.ActiveMessageBox(MessageBoxType.End);
            }

            else if (newStatus == GameStatus.Paused)
            {
                _ennemyManager.Enabled = false;

                _player.Enabled = false;

                gameUI.Enabled = false;

                messageBoxManager.ActiveMessageBox(MessageBoxType.Pause);
            }

            else if (newStatus == GameStatus.Running)
            {
                _ennemyManager.Enabled = true;

                _player.Enabled = true;

                gameUI.Active = true;
                messageBoxManager.DesactiveMessageBoxes();
            }

            else if (newStatus == GameStatus.Starting)
            {
                Registry.AudioManager.SpeakAsync("Get ready !");

                _ennemyManager.Enabled = false;

                _player.Enabled = false;

                messageBoxManager.ActiveMessageBox(MessageBoxType.Start);
            }

            else if (newStatus == GameStatus.Success)
            {
                Registry.AudioManager.StopMusic();
                YnG.SwitchState(new ScoreState(true));
            }

            else if (newStatus == GameStatus.Quit)
            {
                Registry.AudioManager.StopMusic();
                YnG.SwitchState(new ScoreState(true));
            }

            else if (newStatus == GameStatus.Died)
            {
                Registry.AudioManager.StopMusic();
                YnG.SwitchState(new ScoreState(true));
            }

            gameStatus = newStatus;

            if (restart)
                RestartScene();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}

