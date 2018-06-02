using System;
using Microsoft.Xna.Framework;
using Yna.Engine;
using Yna.Engine.Graphics;
using SpaceGame.Player;

namespace SpaceGame.Level
{
    public enum BonusType
    {
        Laser = 0, Missile, Ammo, Debris
    }

    public class BaseBonus : YnSprite
    {
        private BonusType _type;

        public BaseBonus(BonusType type, int x, int y)
        {
            _type = type;

            switch (type)
            {
                case BonusType.Ammo:     _assetName = "Misc/Bonus";    break;
                case BonusType.Debris:   _assetName = "Misc/Bonus/BonusDebris";  break;
                case BonusType.Laser:    _assetName = "Misc/Bonus/BonusLaser";   break;
                case BonusType.Missile:  _assetName = "Misc/Bonus/BonusMissile";  break;
            }

            _position = new Vector2(x, y);

            LoadContent();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            //SetOriginTo(ObjectOrigin.Center);
        }

        public override void Initialize()
        {
            base.Initialize();

            int[] indexes;
            Vector2 origin;

            if (_type == BonusType.Debris)
            {
                indexes = new int[] { 0, 1, 2, 3 };
                Origin = new Vector2(Texture.Width / 8, Texture.Height / 2);
            }
            else
            {
                indexes = new int[] { 0 };
                SetOrigin(SpriteOrigin.Center);
            }

            PrepareAnimation(24, 24);
            AddAnimation("0", indexes, 100, false);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Rotation += 0.05f;

            Y += 2; 

            Play("0");

            if (Registry.Players.Length > 1)
            {
                foreach (SpacePlayer player in Registry.Players)
                {
                    if (this.Rectangle.Intersects(player.Rectangle))
                        Kill((int)player.PlayerIndex);
                }
            }
            else
            {
                if (this.Rectangle.Intersects(Registry.Players[0].Rectangle))
                    Kill((int)Registry.Players[0].PlayerIndex);
            }
        }

        public void Kill(int player)
        {
            base.Kill();
            if (_type == BonusType.Ammo)
                Registry.Players[player].IncrementBonus();

            Registry.Players[player].GameScore.Score += 20;
        }
    }
}
