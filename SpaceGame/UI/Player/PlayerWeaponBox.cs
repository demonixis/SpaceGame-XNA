using System;
using Microsoft.Xna.Framework;
using Yna.Engine;
using Yna.Engine.Graphics;
using SpaceGame.Player;

namespace SpaceGame.UI.Player
{
    public class PlayerWeaponBox : YnGroup
    {
        private YnText _playerLabel;
        private WeaponItem _primaryWeaponType;
        private WeaponItem _secondaryWeaponType;
        private YnGroup _groupWeapon;
        private ProgressBar _healthBar;
        private ProgressBar _shieldBar;

        public YnGroup Weapons
        {
            get { return _groupWeapon; }
        }

        public ProgressBar HealthBar
        {
            get { return _healthBar; }
        }

        public ProgressBar ShieldBar
        {
            get { return _shieldBar; }
        }

        public PlayerWeaponBox(int x, int y, SpacePlayer player, ref Vector2 scale)
        {
            X = x;
            Y = y;

            _playerLabel = new YnText("Fonts/HUD", player.PlayerName, new Vector2(X, Y), Color.Green);
            Add(_playerLabel);

            _healthBar = new ProgressBar(0, 0, "Energy");
            Add(_healthBar);

            _shieldBar = new ProgressBar(0, 0, "Shield");
            Add(_shieldBar);

            _primaryWeaponType = new WeaponItem(0, 0, "Red", Color.Blue, Color.DarkCyan, Assets.LaserBlue, player.PrimaryWeaponType, true);
            Add(_primaryWeaponType);

            _secondaryWeaponType = new WeaponItem(0, 0, "Red", Color.Blue, Color.DarkCyan, Assets.Missile, player.SecondaryWeaponType, true);
            Add(_secondaryWeaponType);
        }

        public override void Initialize()
        {
            base.Initialize();
            int offset = 10;
            _playerLabel.Position = new Vector2(X, Y);
            _healthBar.Position = new Vector2(X, Y + _playerLabel.Height + offset);
            _shieldBar.Position = new Vector2(_healthBar.X + _healthBar.Width + 5, _healthBar.Y);
            _primaryWeaponType.Position = new Vector2(X, _shieldBar.Y + _shieldBar.Height + offset);
            _secondaryWeaponType.Position = new Vector2(_primaryWeaponType.X + _primaryWeaponType.Width + 4, _primaryWeaponType.Y);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
    }
}
