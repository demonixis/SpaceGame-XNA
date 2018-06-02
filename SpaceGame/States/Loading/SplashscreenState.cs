using System;
using Microsoft.Xna.Framework;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Animation;
using Yna.Engine.Graphics.Event;
using Yna.Engine.State;
using SpaceGame.States.Menu;

namespace SpaceGame.States.Loading
{
    public class SplashscreenState : YnState2D
    {
        private YnEntity background;
        private YnTimer timer;
        private YnTransitionEffect transition;

        public SplashscreenState() 
            : base ("splash", true)
        {
            background = new YnEntity("Backgrounds/Splash");
            Add(background);
            
            timer = new YnTimer(3000, 0);
            timer.Completed += timer_Completed;
        }

        void timer_Completed()
        {
            YnG.SwitchState(new MenuState());
        }

        public override void Initialize()
        {
            base.Initialize();
            
            background.Rectangle = new Rectangle(0, 0, YnG.Width, YnG.Height);

            timer.Start();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            timer.Update(gameTime);
        }
    }
}
