using System;
using Microsoft.Xna.Framework;
using Yna.Engine;
using Yna.Engine.Graphics;


namespace SpaceGame.Level.Scrolling
{
    public class StarsScrolling : YnGroup
    {
        private YnSprite _background;
        private int _numStars;

        public int NumStars
        {
            get { return _numStars; }
            set { _numStars = value; }
        }

        public StarsScrolling()
            : base()
        {
            _background = new YnSprite(new Rectangle(0, 0, YnG.Width, YnG.Height), Color.Black);
            Add(_background);

            _numStars = 100;
        }

        public override void Initialize()
        {
            base.Initialize();

            Random rand = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < _numStars; i++)
            {
                Color color = Color.Gray;
                Rectangle rectangle = new Rectangle(0, 0, 2, 2);

                switch (rand.Next(6))
                {
                    case 0:
                        rectangle = new Rectangle(0, 0, 1, 1);
                        color = Color.Yellow;
                        break;
                    case 1:
                        rectangle = new Rectangle(0, 0, 2, 2);
                        color = Color.Yellow;
                        break;
                    case 2:
                        rectangle = new Rectangle(0, 0, 1, 1);
                        color = Color.DarkGray;
                        break;
                    case 3:
                        rectangle = new Rectangle(0, 0, 2, 2);
                        color = Color.DarkGray;
                        break;
                    case 4:
                        rectangle = new Rectangle(0, 0, 1, 1);
                        color = Color.AntiqueWhite;
                        break;
                    case 5:
                        rectangle = new Rectangle(0, 0, 2, 2);
                        color = Color.AntiqueWhite;
                        break;
                }

                YnSprite star = new YnSprite(rectangle, color);
                star.Position = new Vector2(rand.Next(YnG.Width), rand.Next(YnG.Height));
                star.Velocity = new Vector2(0, (float)(rand.NextDouble() * 1.5));
                star.AllowAcrossScreen = true;
                Add(star);
            }
        }

        
    }
}
