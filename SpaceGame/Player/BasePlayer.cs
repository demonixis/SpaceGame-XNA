using System;
using Microsoft.Xna.Framework;
using Yna.Engine.Graphics;

namespace SpaceGame.Player
{
    public abstract class BasePlayer : YnSprite
    {
        private float _baseHealth;
        private float _baseShield;
        protected float _health;
        protected float _shield;
        protected float _speed;

        public float BaseHealth
        {
            get { return _baseHealth; }
        }

        public float BaseShield
        {
            get { return _baseShield; }
        }

        public float Health
        {
            get { return _health; }
            set { _health = value; }
        }

        public float Shield
        {
            get { return _shield; }
            set { _shield = value; }
        }

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public BasePlayer()
            : base()
        {
            _health = 100.0f;
            _shield = 100.0f;
            _speed = 1.0f;
            _baseHealth = _health;
            _baseShield = _shield;
        }

        public virtual void AddDamage(float damages)
        {
            // 1 - ce sont les boucliers qui prennent en premier
            _shield -= damages;

            // 2 - ensuite c'est au tour des points de vie
            if (_shield <= 0)
            {
                _health -= damages;

                if (_health <= 0)
                    Kill();
            }
        }

        public abstract void ReceiveDamage(Color color);

        public float GetHealthPercent()
        {
            return (_health * 100) / _baseHealth;
        }

        public float GetShieldPercent()
        {
            return (_shield * 100) / _baseShield;
        }
    }
}
