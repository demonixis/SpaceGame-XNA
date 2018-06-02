using System;
using Microsoft.Xna.Framework;
using SpaceGame.Data.Description;

namespace SpaceGame.Player.Weapon
{
    public class PrimaryWeapon : BaseWeapon
    {
        protected WeaponType _type;
        protected WeaponDescription _description;

        public PrimaryWeapon(WeaponDescription description, Rectangle playerRectangle, int index)
        {
            _index = index;
            _description = description;
            PlayerRectangle = playerRectangle;
            Speed = (int)description.Speed;
            Interval = description.Interval;
            Damage = description.Damage;
            Acceleration = description.Acceleration;
            AssetName = description.AssetName;
        }

        private static float[] _rotationsBurst = new float[]
        {
            -35, -25, -15, 0, 15, 25, 35
        };

        private static Vector2 _directionsLaser = new Vector2(0.0f, -1.0f);

        private static Vector2[] _directionsBurst = new Vector2[]
        {
            new Vector2(-0.35f, -1),
            new Vector2(-0.25f, -1),
            new Vector2(-0.15f, -1), 
            new Vector2(0, -1), 
            new Vector2(0.15f, -1),
            new Vector2(0.25f, -1),
            new Vector2(0.35f, -1)
        };

        private static Vector2[] _directionsSphere = new Vector2[]
        {
            new Vector2(-1.0f, 0.0f),
            new Vector2(-1.0f, 0.0f),
            new Vector2(0.0f, -1.0f), 
            new Vector2(0.0f, -1.0f), 
            new Vector2(0.0f, -1.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(1.0f, 0.0f)
        };

        public override void Initialize()
        {
            base.Initialize();

            // Mise en place de la direction suivant l'index
            if (_description.WeaponType == WeaponType.Burst)
            {
                _direction = _directionsBurst[_index];
                _rotation = _rotationsBurst[_index];
                Scale = new Vector2(0.4f); // TODO Virer ça
            }
            else if (_description.WeaponType == WeaponType.Sphere)
                _direction = _directionsSphere[_index];
            else
                _direction = _directionsLaser;

            // Calcul de la position
            int offset = (_index - 3) * 10;

            Position = new Vector2(
                (PlayerRectangle.X + PlayerRectangle.Width / 2 - Width / 2) + offset,
                (PlayerRectangle.Y - Height / 2));

            Velocity = new Vector2(Direction.X * Speed, Direction.Y * Speed);
        }

        public override void PlaySound()
        {
            if (_description.SoundAsset != String.Empty && _index == 3)
                Registry.AudioManager.PlaySound(_description.SoundAsset, 1.0f);
        }
    }
}
