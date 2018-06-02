using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Yna.Engine;
using Yna.Engine.Graphics;
using SpaceGame.Ennemies;

namespace SpaceGame.Player.Weapon
{
    public abstract class BaseWeapon : YnSprite
    {
        private int _damage;
        private int _speed;
        private int _interval;
        private Rectangle _playerRectangle;
        protected int _index;

        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        public int Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public int Interval 
        {
            get { return _interval; }
            set { _interval = value; }
        }

        public Rectangle PlayerRectangle
        {
            get { return _playerRectangle; }
            set { _playerRectangle = value; }
        }

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Active)
            {
                foreach (Ennemy ennemy in Registry.Ennemies.Members)
                {
                    if (ennemy.Active && this.Rectangle.Intersects(ennemy.Rectangle))
                    {
                        Kill();
                        ennemy.AddDamage(Damage);
                        ennemy.ReceiveDamage(Color.Red);
                    }
                }
            }

            if (Y <= Viewport.Y || X < Viewport.X || X + Width > Viewport.Width)
                Kill();
        }

        public virtual void Reset(Rectangle shipRectangle, int index)
        {
            _playerRectangle = shipRectangle;
            _index = index;
            
            Initialize();
            Revive();
        }

        public abstract void PlaySound();
    }
}
