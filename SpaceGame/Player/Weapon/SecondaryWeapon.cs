using System;
using Microsoft.Xna.Framework;

namespace SpaceGame.Player.Weapon
{
    public class SecondaryWeapon : BaseWeapon
    {
        public SecondaryWeapon(Rectangle parent)
        {
            _assetName = Assets.Missile;

            PlayerRectangle = parent;
            
            Damage = 105;
            Interval = 1500;
            Speed = 6;
        }

        public override void Initialize()
        {
            base.Initialize();

            Position = new Vector2((PlayerRectangle.X + (PlayerRectangle.Width / 2)) - (Width / 2), PlayerRectangle.Y - Height); 
            VelocityY = -Speed;
            Acceleration = new Vector2(0, 2.0f);
        }

        public override void PlaySound()
        {
            //throw new NotImplementedException();
        }
    }
}
