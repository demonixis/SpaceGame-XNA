using System;
using Microsoft.Xna.Framework;
using Yna.Engine;
using Yna.Engine.Graphics;
using SpaceGame.Player;
using SpaceGame.UI.Player;
using SpaceGame.Data.Description;

namespace SpaceGame.UI
{
    public class GameUI : YnGroup
    {
        private YnGroup _guiGroup;
        private YnText _scoreText;
        private YnText _timeText;

        public int minutes;
        public int seconds;
        public int milliseconds;

        private PlayerWeaponBox[] _playerWeaponBox;

        public GameUI(SpacePlayer [] players)
        {
            int nbPlayer = players.Length;

            minutes = 0;
            seconds = 0;
            milliseconds = 0;

            Vector2 scale = SpaceGame.GetScale();

            _guiGroup = new YnGroup(2);
            _scoreText = new YnText("Fonts/ScoresUI", "0");
            _scoreText.Color = Color.Wheat;
            Add(_scoreText);

            _timeText = new YnText("Fonts/ScoresUI", "00:00:000");
            _timeText.Color = Color.Wheat;
            Add(_timeText);

            _playerWeaponBox = new PlayerWeaponBox[nbPlayer];

            for (int i = 0; i < nbPlayer; i++)
            {
                // Si 2 joueurs on a un alignement en bas
                int x = (i * 1100) + 25;
                int y = 700;

                // Sinon les joueurs 1 et 2 sont en haut
                // Et les autres en bas
                if (nbPlayer > 2)
                {
                    // Joueurs 1 et 2
                    if (i < 2)
                        y = 25;

                    // Joueurs 3 et 4
                    else
                    {
                        x = ((i - 2) * 1100) + 25;
                        y = 700;
                    }
                }

                _playerWeaponBox[i] = new PlayerWeaponBox(x, y, players[i], ref scale);
                Add(_playerWeaponBox[i]);
            }
        }

        public void ChangeWeapon(WeaponType weapon, int playerIndex)
        {
            foreach (WeaponItem item in _playerWeaponBox[playerIndex].Weapons)
            {
                if (item.Identifier != weapon)
                    item._selector.Visible = false;
                else
                    item._selector.Visible = true;
            }
        }

        public void UpdateHealthSatus(int playerIndex, float value)
        {
            _playerWeaponBox[playerIndex].HealthBar.UpdateProgession(value);
        }

        public void UpdateShieldStatus(int playerIndex, float value)
        {
            _playerWeaponBox[playerIndex].ShieldBar.UpdateProgession(value);
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Initialize()
        {
            base.Initialize();

            _timeText.Position = new Vector2(
               (float)(YnG.Width / 2.5),
               (float)5.0f);

            _scoreText.Position = new Vector2(
                (float)(_timeText.X + _timeText.Width + 25.0f),
                5.0f);

            Reset();
        }

        public void Reset()
        {
            _timeText.Text = "00:00:000";
            _scoreText.Text = "0";
        }

        public override void Revive()
        {
            base.Revive();

            int countPlayer = Registry.Players.Length;
            if (countPlayer > 1)
            {
                for (int i = 0; i < countPlayer; i++)
                {
                    UpdateHealthSatus(i, 100);
                    UpdateShieldStatus(i, 100);
                }
            }
            else
            {
                UpdateHealthSatus(0, 100);
                UpdateShieldStatus(0, 100);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            string minutes = gameTime.TotalGameTime.Minutes > 9 ? gameTime.TotalGameTime.Minutes.ToString() : "0" + gameTime.TotalGameTime.Minutes.ToString();
            string secondes = gameTime.TotalGameTime.Seconds > 9 ? gameTime.TotalGameTime.Seconds.ToString() : "0" + gameTime.TotalGameTime.Seconds.ToString();
            string ms = gameTime.TotalGameTime.Milliseconds.ToString();

            _timeText.Text = String.Format("{0:2}:{1:2}:{2:1}", minutes, secondes, ms);
            _scoreText.Text = String.Format("{0} ", Registry.Players[0].GameScore.Score); // TODO HACK
        }
    }
}
