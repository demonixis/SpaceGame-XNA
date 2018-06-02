using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Animation;

namespace SpaceGame.UI
{
    public class ProgressBar : YnGroup
    {
        private YnText label;
        private YnSprite backgroundBar;
        private YnSprite foregroundBar;
        private YnTransitionEffect transition;
        private int borderSize;

        public Color BackgroundBarColor
        {
            get { return backgroundBar.Color; }
            set { backgroundBar.Color = value; }
        }

        public Color ForegroundBarColor
        {
            get { return foregroundBar.Color; }
            set { foregroundBar.Color = value; }
        }

        public ProgressBar(int x, int y, string labelText)
        {
            borderSize = 1;

            backgroundBar = new YnSprite(new Rectangle(x, y, 130, 20), Color.White);
            Add(backgroundBar);

            foregroundBar = new YnSprite(new Rectangle(x + borderSize, y + borderSize, backgroundBar.Width - (2 * borderSize), backgroundBar.Height - (2 * borderSize)), Color.Blue);
            Add(foregroundBar);

            label = new YnText("Fonts/HUD", labelText);
            label.Scale = new Vector2(0.80f, 0.80f);
            label.Color = Color.White;
            Add(label);
        }

        public override void Initialize()
        {
            base.Initialize();

            label.Position = new Vector2((backgroundBar.X + (backgroundBar.Width / 2)) - (label.Width / 2), (backgroundBar.Y + (backgroundBar.Height / 2)) - (label.Height / 2));
        }

        public void UpdateProgession(float progression)
        {
            float scaleX = progression / 100;
            if (scaleX >= 0.0f)
                foregroundBar.Scale = new Vector2(scaleX, foregroundBar.Scale.Y);
        }

        public new Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                backgroundBar.Position = value;
                foregroundBar.Position = new Vector2(value.X + borderSize, value.Y + borderSize);
                label.Position = new Vector2((backgroundBar.X + (backgroundBar.Width / 2)) - (label.Width / 2), (backgroundBar.Y + (backgroundBar.Height / 2)) - (label.Height / 2));
            }
        }

        public new int Width
        {
            get { return backgroundBar.Width; }
        }

        public new int Height
        {
            get { return backgroundBar.Height; }
        }
    }
}
