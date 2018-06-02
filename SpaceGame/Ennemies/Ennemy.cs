using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Yna.Engine;
using Yna.Engine.Graphics;
using SpaceGame.Player;
using SpaceGame.Data.Description;

namespace SpaceGame.Ennemies
{
    public class Ennemy : BasePlayer
    {
        private YnTimer _timerFired;
        private EnnemyType _type;
        public float _center;
        private float _radius;
        private float _sinAngle;
        private int _framerate;
        private Vector2 _animationSize;
        private int[] _animationIndex;
        private EnnemyDescription _description;

        public float Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        public float Center
        {
            get { return _center; }
            set { _center = value; }
        }

        public EnnemyType EnnemyType
        {
            get { return _type; }
        }

        public Ennemy(EnnemyDescription description)
        {
            _type = (EnnemyType)description.Id;
            _description = description;
            AssetName = description.AssetName;
            _framerate = description.Framerate;
            _animationSize = new Vector2(description.AnimationSize[0], description.AnimationSize[1]);
            _animationIndex = description.AnimationIndex;
            _speed = description.Speed;
            _health = description.Health;
            _shield = 0.0f;
            _scale = new Vector2(0.75f);
            _timerFired = new YnTimer(100);
            _timerFired.Completed += timerFired_Completed;

            _radius = 60.0f;
            _sinAngle = 0.0f;

            LoadContent();
        }

        protected void timerFired_Completed()
        {
            Color = Color.White;
        }

        public override void Revive()
        {
            base.Revive();

            _health = _description.Health;
            _shield = _description.Shield;
        }

        public void Kill(int playerIndex)
        {
            if (Health <= 0)
                Registry.Players[playerIndex].GameScore.Score += 10;
            else
                Registry.Players[playerIndex].GameScore.Score -= 5;

            if (Registry.Players[playerIndex].GameScore.Score < 0)
                Registry.Players[playerIndex].GameScore.Score = 0;

            Registry.FX.AddExplosion(Rectangle);

            base.Kill();
        }

        public override void AddDamage(float damages)
        {
            // 1 - ce sont les boucliers qui prennent en premier
            _shield -= damages;

            // 2 - ensuite c'est au tour des points de vie
            if (_shield <= 0)
                _health -= damages;
        }

        public override void ReceiveDamage(Color color)
        {
            Color = color;
            _timerFired.Start();
        }

        #region GameState pattern

        public override void LoadContent()
        {
            base.LoadContent();

            PrepareAnimation((int)_animationSize.X, (int)_animationSize.Y);
            AddAnimation("idle", new int[] { 0 }, 0, false);
            AddAnimation("move", _animationIndex, _framerate, false);
        }

        public void Initialize(Vector2 position)
        {
            Position = position;

            Center = position.X;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _timerFired.Update(gameTime);

            _position.X = (float)(Center + Math.Sin(Y * 0.01f) * _radius);
            _position.Y += _speed;

            Play("move");

            int countPlayers = Registry.Players.Length;

            if (countPlayers > 1)
            {
                foreach (SpacePlayer player in Registry.Players)
                {
                    if (Rectangle.Intersects(player.Rectangle))
                        AddDamagesOnPlayer(player, 30);

                    if (Health <= 0)
                        Kill((int)player.PlayerIndex);
                }
            }
            else
            {
                if (Rectangle.Intersects(Registry.Players[0].Rectangle))
                    AddDamagesOnPlayer(Registry.Players[0], 30);

                if (Health <= 0)
                    Kill(0);
            }

            if (Y > YnG.Height)
                Kill(0);
        }

        private void AddDamagesOnPlayer(SpacePlayer player, float damages)
        {
            if (player.CanBeFired)
            {
                player.AddDamage(damages);
                player.ReceiveDamage(Color.Blue);
            }
        }

        #endregion
    }
}
