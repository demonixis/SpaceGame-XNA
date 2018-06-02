using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Animation;
using Yna.Engine.Graphics.Event;
#if COMPLETE
using Yna.Input.Kinect;
#endif
using SpaceGame.UI;
using SpaceGame.UI.Menu;

namespace SpaceGame.States.Menu
{
    public class CreditsState : BaseMenu
    {
        YnEntity background;

        public CreditsState()
            : base("Credits", 1)
        {
            background = new YnEntity("Backgrounds/credits");
            Add(background);

            Rectangle rectangle = new Rectangle(
                YnG.Width / 2 - MenuItem.SmallItemWidth / 2,
                YnG.Height - (int)SpaceGame.GetScaleY(75),
                MenuItem.SmallItemWidth,
                MenuItem.SmallItemHeight);

            MenuItem item = new MenuItem("Menu", rectangle, 0, false);
            item.ItemTextSize = SpaceGame.GetScale();

            item.MouseClicked += new EventHandler<MouseClickEntityEventArgs>(item_MouseJustClicked);
            item.MouseLeave += new EventHandler<MouseLeaveEntityEventArgs>(item_MouseLeave);
            item.MouseOver += new EventHandler<MouseOverEntityEventArgs>(item_MouseOver);
            Add(item);

            items.Add(item);
        }

        public override void Initialize()
        {
            base.Initialize();

            _title.Position = new Vector2(50, 25);

            Registry.AudioManager.SpeakAsync("Who are behind, this awesome game ?");

            background.Scale = SpaceGame.GetScale();
            background.Position = new Vector2(YnG.Width / 2 - background.Width / 2, YnG.Height / 2 - background.Height / 2);
        }

        protected override void item_MouseJustClicked(object sender, MouseClickEntityEventArgs e)
        {
            YnG.SwitchState(new MenuState());
        }
    }
}
