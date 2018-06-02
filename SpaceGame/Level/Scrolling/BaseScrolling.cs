using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Yna.Engine;
using Yna.Engine.Graphics;

namespace SpaceGame.Level.Scrolling
{
    public abstract class BaseScrolling : YnGroup
    {
        protected bool _inAcceleration;
        protected int _acceleration;
        protected Rectangle _playableViewport;

        public bool InAcceleration
        {
            get { return _inAcceleration; }
        }

        public int Acceleration
        {
            get { return _acceleration; }
            set { _acceleration = value; }
        }

        public Rectangle PlayableViewport
        {
            get { return _playableViewport; }
        }

        public BaseScrolling()
            : base()
        {
            _inAcceleration = false;
            _acceleration = 4;
            _playableViewport = new Rectangle(0, 0, YnG.Width, YnG.Height);
        }

        public abstract void ScrollUp();
        public abstract void ScrollDown();

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
