using System;
using Microsoft.Xna.Framework;
using Yna.Engine;
using Yna.Engine.Graphics;
using SpaceGame.Level.Scrolling;

namespace SpaceGame.Level
{
    public class SpaceBackground : BaseScrolling
    {
        private StarsScrolling stars;
        private PlanetsScrolling planets;

        public SpaceBackground()
        {
            stars = new StarsScrolling();
            Add(stars);

            planets = new PlanetsScrolling();
            Add(planets);
        }

        public override void ScrollUp()
        {
            if (!_inAcceleration)
            {
                foreach (YnSprite sprite in stars)
                    sprite.VelocityY += _acceleration;

                _inAcceleration = true;
            }
        }

        public override void ScrollDown()
        {
            if (_inAcceleration)
            {
                foreach (YnSprite sprite in stars)
                    sprite.VelocityY -= _acceleration;

                _inAcceleration = false;
            }
        }
    }
}