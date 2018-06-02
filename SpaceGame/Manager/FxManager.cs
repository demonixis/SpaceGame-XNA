using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.State;


namespace SpaceGame.Manager
{
    public class FxManager : YnGroup
    {
        public Stack<int> _recycled;

        public FxManager()
        {
            _recycled = new Stack<int>();
        }

        public void AddExplosion(Rectangle rectangle)
        {
            YnSprite explode;

            int recycledSize = _recycled.Count;

            if (recycledSize > 0)
            {
                explode = Members.ElementAt(_recycled.Pop()) as YnSprite;

                explode.Position = new Vector2(
                    (rectangle.X + rectangle.Width / 2) - (explode.Width / 5) / 2,
                    (rectangle.Y + rectangle.Height / 2) - explode.Height / 2);
                
                explode.Revive();
            }
            else
            {
                explode = new YnSprite("SFX/Fx_Fireball");
                explode.LoadContent();
                explode.PrepareAnimation(94, 94);
                explode.AddAnimation("0", new int[] { 0, 1, 2, 3, 4 }, 50, false);
                explode.Scale = new Vector2(0.75f);

                explode.Position = new Vector2(
                    (rectangle.X + rectangle.Width / 2) - (explode.Width / 5) / 2,
                    (rectangle.Y + rectangle.Height / 2) - explode.Height / 2);

                // On supprime l'animation quand elle est terminée
                explode.GetAnimation("0").AnimationComplete += (sender, e) =>
                {
                    explode.Kill();
                    _recycled.Push(Members.IndexOf(explode));
                };

                Add(explode);
            } 
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (Count > 0)
            {
                foreach (YnSprite fx in Members)
                {
                    if (fx.Active)
                        fx.Play("0");   
                }
            }
        }
    }
}
