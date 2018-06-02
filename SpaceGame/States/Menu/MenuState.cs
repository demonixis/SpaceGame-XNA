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
using SpaceGame.Data.Description;
using SpaceGame.Data;

namespace SpaceGame.States.Menu
{
    public class MenuState : BaseMenu
    {
        private YnEntity _kinectLogo;
        private YnEntity _planetLogo;
        private YnSprite _shipSprite;

        public MenuState()
            : base("Codename : SpaceGame", 6)
        {
            // Les elements qui passent sous le menu
            _planetLogo = new YnEntity(Assets.PlanetRed);
            Add(_planetLogo);

            _shipSprite = new YnSprite(Assets.PlayerShipA);
            Add(_shipSprite);

            _kinectLogo = new YnEntity("Misc/kinect");
            Add(_kinectLogo);

            // 3 - Le menu
            string[] itemNames = { "Arcade", "Multiplayers", "Scores", "Options", "Credits", "Exit" };

            int x = (YnG.Width / 2) - MenuItem.BigItemWidth / 2;
            int y = 0;

            for (int i = 0; i < 3; i++)
            {
                y = (int)SpaceGame.GetScaleY(185) + i * MenuItem.BigItemHeight * 2;

                MenuItem item = new MenuItem(itemNames[i], new Rectangle(
                    (int)(x), 
                    (int)(y), 
                    MenuItem.BigItemWidth, 
                    MenuItem.BigItemHeight), 
                    i, 
                    i == 0 ? true : false);

                item.ItemTextSize = SpaceGame.GetScale();
                item.MouseClicked += new EventHandler<MouseClickEntityEventArgs>(item_MouseJustClicked);
                item.MouseLeave += new EventHandler<MouseLeaveEntityEventArgs>(item_MouseLeave);
                item.MouseOver += new EventHandler<MouseOverEntityEventArgs>(item_MouseOver);
                Add(item);

                items.Add(item);
            }

            x = 0;
            y = YnG.Height - (MenuItem.SmallItemHeight + MenuItem.SmallItemHeight / 3);

            for (int j = 3; j < 6; j++)
            {
                x = (int)SpaceGame.GetScaleX(350) + (j - 3) * MenuItem.SmallItemWidth * 2;

                MenuItem item = new MenuItem(itemNames[j], new Rectangle(x, y, MenuItem.SmallItemWidth, MenuItem.SmallItemHeight), j, false);
                item.ItemTextSize = new Vector2(0.6f) * SpaceGame.GetScale();
                item.MouseClicked += new EventHandler<MouseClickEntityEventArgs>(item_MouseJustClicked);
                item.MouseLeave += new EventHandler<MouseLeaveEntityEventArgs>(item_MouseLeave);
                item.MouseOver += new EventHandler<MouseOverEntityEventArgs>(item_MouseOver);
                Add(item);

                items.Add(item);
            }
        }

        #region Evenements souris

        protected override void item_MouseJustClicked(object sender, MouseClickEntityEventArgs e)
        {
            MenuItem item = (sender as MenuItem);

            if (item != null)
            {
                switch (item.ItemPosition)
                {
                    case 0: YnG.SwitchState(new SelectShipState()); break;
                    case 1:
                        Registry.AudioManager.SpeakAsync("This section is disabled at this time");
                        //YnG.SwitchState(new MultiPlayerState(2)); 
                        break;
                    case 2: YnG.SwitchState(new ScoreState()); break;
                    case 3: YnG.SwitchState(new OptionsState()); break;
                    case 4: YnG.SwitchState(new CreditsState()); break;
                    case 5: YnG.Exit(); break;
                }
            }
        }

        #endregion

        public override void Initialize()
        {
            base.Initialize();
            Registry.AudioManager.PlayMusic("Audio/DST-DustLoop");
            Registry.AudioManager.VocalRate = -2;
            Registry.AudioManager.SpeakAsync("Welcome Commander !");

            _title.Position = new Vector2(YnG.Width / 2 - _title.Width / 2, 25);

            _kinectLogo.Position = new Vector2(YnG.Width - _kinectLogo.Width - 10, YnG.Height - _kinectLogo.Height - 10);

            _planetLogo.Scale = new Vector2(1.2f, 1.2f);
            _planetLogo.Position = new Vector2(-30, YnG.Height - _planetLogo.Height / 2);

            _shipSprite.LoadContent();
            _shipSprite.Position = new Vector2(195, _planetLogo.Y - 95);
            _shipSprite.PrepareAnimation(64, 64);
            _shipSprite.AddAnimation("fire", new int[] { 1, 2 }, 370, false);
            _shipSprite.Rotation = MathHelper.ToRadians(30);
            _shipSprite.Scale = new Vector2(1.5f);
        }
    }
}
