using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Helpers;
using SpaceGame.Level.Scrolling;

namespace SpaceGame.Level
{
    public class InteriorBackground : BaseScrolling
    {
        YnSprite borderLeft;
        YnSprite borderRight;
        YnSprite angularLeft;
        YnSprite angularRight;
        YnSprite center;
        float position;
        int borderSize;

        public Rectangle Viewport
        {
            get
            {
                return new Rectangle((int)angularLeft.X + angularLeft.Width / 2, 0, (int)angularRight.X + angularRight.Width / 2, YnG.Height);
            }
        }

        public InteriorBackground()
            : base()
        {
            position = 0;
            borderSize = 75;

            borderLeft = new YnSprite(new Rectangle(0, 0, borderSize, YnG.Height), new Color(64, 64, 64));
            borderRight = new YnSprite(new Rectangle(YnG.Width - borderSize, 0, borderSize, YnG.Height), new Color(64, 64, 64));

            angularLeft = new YnSprite(new Rectangle(borderSize, 0, borderSize, YnG.Height), new Color(48, 48, 48));
            angularRight = new YnSprite(new Rectangle(YnG.Width - (2 * borderSize), 0, borderSize, YnG.Height), new Color(48, 48, 48));

            center = new YnSprite(new Rectangle((2 * borderSize), 0, YnG.Width - (4 * borderSize), YnG.Height), new Color(32, 32, 32));

            _playableViewport = new Rectangle((int)angularLeft.X + angularLeft.Width / 2, 0, (int)angularRight.X + angularRight.Width / 2, YnG.Height);
        }

        private void DrawLines(SpriteBatch spriteBatch, int y = 50, int offset = 30)
        {
            Texture2D blank = YnGraphics.CreateTexture(Color.White, 1, 1);

            float percent = (((100.0f * (float)(y - (offset * 4))) / ((float)YnG.Height / 2))) / 100.0f;
            float direction = -1.0f;

            offset = (int)(offset * percent * direction);

            YnGraphics.DrawLine(spriteBatch, blank, 2, new Color(100, 100, 100), new Vector2(borderLeft.X, YnG.Height - y), new Vector2(borderLeft.X + borderLeft.Width, YnG.Height - y));
            YnGraphics.DrawLine(spriteBatch, blank, 1, new Color(75, 75, 75), new Vector2(borderLeft.X + borderLeft.Width, YnG.Height - y), new Vector2(angularLeft.X + angularLeft.Width, YnG.Height - (y + offset)));
            YnGraphics.DrawLine(spriteBatch, blank, 1, new Color(50, 50, 50), new Vector2(angularLeft.X + angularLeft.Width, YnG.Height - (y + offset)), new Vector2(center.X + center.Width, YnG.Height - (y + offset)));
            YnGraphics.DrawLine(spriteBatch, blank, 1, new Color(75, 75, 75), new Vector2(center.X + center.Width, YnG.Height - (y + offset)), new Vector2(angularRight.X + angularRight.Width, YnG.Height - y));
            YnGraphics.DrawLine(spriteBatch, blank, 2, new Color(100, 100, 100), new Vector2(angularRight.X + angularRight.Width, YnG.Height - y), new Vector2(borderRight.X + borderRight.Width, YnG.Height - y));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (position <= -900)
                position = 0;
            else
                position -= 3;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            
            borderLeft.Draw(gameTime, spriteBatch);
            borderRight.Draw(gameTime, spriteBatch);
            angularLeft.Draw(gameTime, spriteBatch);
            angularRight.Draw(gameTime, spriteBatch);
            center.Draw(gameTime, spriteBatch);

            for (int i = 0; i < 7; i++)
            {
                DrawLines(spriteBatch, (int)position + 300 * i, borderSize);
            }

        }

        public override void ScrollUp()
        {
            position -= 7;
        }

        public override void ScrollDown()
        {
            
        }
    }
}
