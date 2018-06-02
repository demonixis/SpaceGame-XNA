using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine;
using Yna.Engine.Graphics;
using SpaceGame.Player.Weapon;
using SpaceGame.Data;
using SpaceGame.Data.Description;
#if COMPLETE
using Yna.Input.Kinect;
#endif

namespace SpaceGame.Player
{
    public enum BonusLevel
    {
        None = 0, Level1, Level2, Level3, Level4
    }

    public class PlayerProfile
    {
        public PlayerProfile(string name, int index)
        {
            PlayerIndex = index;
            Name = name;
            GameScore = new GameScore(name, index);
        }

        public int PlayerIndex { get; set; }
        public GameScore GameScore { get; set; }
        public string Name { get; set; }
    }

    public class SpacePlayer : BasePlayer
    {
#if COMPLETE
        protected KinectPlayerIndex _kinectPlayerIndex;
        protected KinectSensorController kinect;
#endif
        // Input
        private PlayerIndex _playerIndex;
        private ShipSpeed _shipSpeed;
        private WeaponManager _weaponManager;
        private YnTimer _playerTouchedTimer;
        private WeaponType _firstWeaponType;
        private WeaponType _secondWeaponType;
        private PlayerProfile _playerProfile;
        private int _live;
        private YnSprite _shields;
        private BonusLevel _bonusLevel;

        #region Propriétés

        public PlayerIndex PlayerIndex
        {
            get { return _playerIndex; }
        }

        public YnGroup Weapons
        {
            get { return _weaponManager; }
        }

        public bool CanBeFired
        {
            get { return !_playerTouchedTimer.Active; }
        }

        public BonusLevel BonusLevel
        {
            get { return _bonusLevel; }
            set { _bonusLevel = value; }
        }

        public GameScore GameScore
        {
            get { return _playerProfile.GameScore; }
            protected set { _playerProfile.GameScore = value; }
        }

        public string PlayerName
        {
            get { return _playerProfile.Name; }
            set { _playerProfile.Name = value; }
        }

        public int Live
        {
            get { return _live; }
            set { _live = value; }
        }

        public WeaponType PrimaryWeaponType
        {
            get { return _firstWeaponType; }
        }

        public WeaponType SecondaryWeaponType
        {
            get { return _secondWeaponType; }
        }

        #endregion

        #region Events

        public event EventHandler<EventArgs> PlayerReallyDead = null;

        protected void PlayerIsReallyDead(EventArgs e)
        {
            if (PlayerReallyDead != null)
                PlayerReallyDead(this, e);
        }

        #endregion

        // TODO ajouter un PlayerBundle ou un PlayerInfo ou un truc dans ce genre
        public SpacePlayer(PlayerIndex playerIndex, SpaceShipDescription description)
            : base()
        {
            AssetName = description.AssetName;

            _playerIndex = playerIndex;

            _shipSpeed = description.ShipSpeed;

            _playerProfile = new PlayerProfile(String.Format("Player {0}", ((int)playerIndex) + 1), (int)playerIndex);

            _firstWeaponType = (WeaponType)description.PrimaryWeaponId;
            _secondWeaponType = (WeaponType)description.SecondaryWeaponId;

            _bonusLevel = BonusLevel.None;

            VelocityMax = 0.95f;
            ForceInsideScreen = true;

            _live = 3;

            // TODO : case
            _shields = new YnSprite("Ship/Shield/Shields");

            _weaponManager = new WeaponManager(this);
            _weaponManager.Initialized = true;
            _weaponManager.AssetsLoaded = true;

            LayerDepth = 0.4f;

            _playerTouchedTimer = new YnTimer(1500, 0);
            _playerTouchedTimer.Completed += _playerTouchedTimer_Completed;

#if COMPLETE
            kinect = KinectSensorController.Instance;

            if (_playerIndex == PlayerIndex.One)
                _kinectPlayerIndex = KinectPlayerIndex.One;
            
            else if (_playerIndex == PlayerIndex.Two)
                _kinectPlayerIndex = KinectPlayerIndex.Two;      
#endif

            LoadContent();
        }

        #region Events

        void _playerTouchedTimer_Completed()
        {
            _shields.Active = false;
            Alpha = 1.0f;
        }

        #endregion

        public override void Initialize()
        {
            base.Initialize();

            _weaponManager.Initialize();

            _shields.LoadContent();
            _shields.PrepareAnimation(128, 128);
            _shields.AddAnimation("smallBlack", new int[] { 0, 1 }, 120, false);
            _shields.AddAnimation("blueStrong", new int[] { 2, 3 }, 120, false);
            _shields.AddAnimation("redStrong", new int[] { 4, 5 }, 120, false);
            _shields.AddAnimation("blueLight", new int[] { 6, 7 }, 120, false);
            _shields.Active = false;

            PrepareAnimation(64, 64);
            AddAnimation("idle", new int[] { 0 }, 0, false);
            AddAnimation("move", new int[] { 1, 2 }, 35, false);
            Reset();
        }

        public override void Revive()
        {
            base.Revive();
            Reset();
        }

        public virtual void Reset()
        {
            // Centrer sur l'éran en bas
            Position = new Vector2((YnG.Width / 2) - (Width / 2), (YnG.Height - (Height * 2)));

            switch (_playerIndex)
            {
                case PlayerIndex.One: X -= YnG.Width / 6; break;
                case PlayerIndex.Two: X += YnG.Width / 6; break;
                case PlayerIndex.Three: X += 2 * (YnG.Width / 6); break;
                case PlayerIndex.Four: X += 3 * (YnG.Width / 6); break;
            }

            Velocity = Vector2.Zero;

            _health = 100;
            _shield = 100;

            _weaponManager.Clear();
            _weaponManager.Revive();

            _shields.Active = false;
        }

        public override void AddDamage(float damages)
        {
            base.AddDamage(damages);

            Registry.GameUI.UpdateHealthSatus((int)_playerIndex, GetHealthPercent());
            Registry.GameUI.UpdateShieldStatus((int)_playerIndex, GetShieldPercent());

            _playerTouchedTimer.Restart();
        }

        public override void ReceiveDamage(Color color)
        {
            _shields.Active = true;
        }

        public override void Kill()
        {
            base.Kill();

            _shields.Active = false;

            _weaponManager.Kill();
            _weaponManager.Clear();

            if (_live > 0)
                _live--;
            else
                PlayerIsReallyDead(EventArgs.Empty);
        }

        public void IncrementBonus()
        {
            int bonus = (int)BonusLevel;
            int size = Enum.GetValues(typeof(BonusLevel)).Length;

            if (++bonus < size)
                this.BonusLevel = (BonusLevel)bonus;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _playerTouchedTimer.Update(gameTime);

            _weaponManager.Update(gameTime);

            // Mise à jour de la position du bouclier
            _shields.X = X + Width / 2 - _shields.Width / 2;
            _shields.Y = Y + Height / 2 - _shields.Height / 2;

            if (_shields.Active)
            {
                if (_shield > 0)
                    _shields.Play("blueStrong");
                else
                    _shields.Play("smallBlack");

                _shields.Update(gameTime);
            }

            #region Gamepad

            if (YnG.Gamepad.Connected(_playerIndex))
            {
                if (YnG.Gamepad.LeftStickUp(_playerIndex))
                    VelocityY -= _shipSpeed.Up * YnG.Gamepad.LeftStickValue(_playerIndex).Y * 1.5f;
                else if (YnG.Gamepad.LeftStickDown(PlayerIndex.One))
                    VelocityY -= _shipSpeed.Down * YnG.Gamepad.LeftStickValue(_playerIndex).Y * 1.5f;

                if (YnG.Gamepad.LeftStickLeft(_playerIndex))
                    VelocityX += _shipSpeed.Left * YnG.Gamepad.LeftStickValue(_playerIndex).X * 1.5f;
                if (YnG.Gamepad.LeftStickRight(_playerIndex))
                    VelocityX += _shipSpeed.Right * YnG.Gamepad.LeftStickValue(_playerIndex).X * 1.5f;

                if (YnG.Gamepad.Pressed(_playerIndex, Buttons.RightTrigger))
                {
                    _weaponManager.Shoot(1);
                }

                if (YnG.Gamepad.JustPressed(_playerIndex, Buttons.LeftTrigger))
                {
                    _weaponManager.Shoot(2);
                }
            }

            #endregion

            #region Clavier

            // Le joueur 2 peut utiliser le clavier si le joueur 1 à une manette de branchée
            if (_playerIndex == PlayerIndex.Two && YnG.Gamepad.Connected(PlayerIndex.One) && !YnG.Gamepad.Connected(PlayerIndex.Two))
                UpdateKeyboardControl();

            else if (_playerIndex == PlayerIndex.One)
                UpdateKeyboardControl();

            #endregion

#if COMPLETE
            #region kinect

            if (kinect.IsAvailable)
            {
                Vector3 handRight = kinect.GetUserProfile(_kinectPlayerIndex).HandRight;
                Vector3 handLeft = kinect.GetUserProfile(_kinectPlayerIndex).HandLeft;

                X = (int)handRight.X;
                Y = (int)handRight.Y;

                if (handLeft.Y > 500)
                    Shoot();

                if (handLeft.Y < 20)
                    SelectedNextWeapon();
            }

            #endregion
#endif
        }

        private void UpdateKeyboardControl()
        {
            // Déplacements
            if (YnG.Keys.Up || YnG.Keys.Down || YnG.Keys.Left || YnG.Keys.Right)
            {
                if (YnG.Keys.Up)
                {
                    VelocityY -= _shipSpeed.Up;
                }

                else if (YnG.Keys.Down)
                    VelocityY += _shipSpeed.Down;

                if (YnG.Keys.Left)
                    VelocityX -= _shipSpeed.Left;

                else if (YnG.Keys.Right)
                    VelocityX += _shipSpeed.Right;

                this.Play("move");
            }
            else
                Play("idle");

            if (YnG.Keys.Space)
                _weaponManager.Shoot(1);

            else if (YnG.Keys.JustPressed(Keys.LeftControl))
                _weaponManager.Shoot(2);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            if (_shields.Active)
                _shields.Draw(gameTime, spriteBatch);

            _weaponManager.Draw(gameTime, spriteBatch);
        }
    }
}
