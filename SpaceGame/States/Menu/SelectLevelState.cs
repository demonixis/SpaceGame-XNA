using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna.Engine;
using Yna.Engine.State;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Event;
using SpaceGame.Player;
using SpaceGame.UI;
using SpaceGame.UI.Player;
using SpaceGame.UI.Menu;
using SpaceGame.Data.Description;

namespace SpaceGame.States.Menu
{
    public class SelectLevelState : BaseMenu
    {
        private int ItemWidth
        {
            get { return (int)SpaceGame.GetScaleX(200); }
        }

        private int ItemHeight
        {
            get { return (int)SpaceGame.GetScaleY(55); }
        }

        private int _selectedLevel;
        private int _selectedShip;

        private YnEntity spaceLevelImage;
        private YnEntity escapeLevelImage;

        public SelectLevelState(int shipType)
            : base("Select a level", 3)
        {
            _selectedShip = shipType;
            _selectedLevel = 0;

            spaceLevelImage = new YnEntity("Backgrounds/selectStars");
            spaceLevelImage.Scale = SpaceGame.GetScale();
            Add(spaceLevelImage);

            escapeLevelImage = new YnEntity("Backgrounds/selectEscape");
            escapeLevelImage.Scale = SpaceGame.GetScale();
            Add(escapeLevelImage);

            Rectangle rectangle = new Rectangle(
                YnG.Width / 2 - MenuItem.SmallItemWidth / 2,
                YnG.Height - (int)SpaceGame.GetScaleY(75),
                MenuItem.SmallItemWidth,
                MenuItem.SmallItemHeight);

            MenuItem backItem = new MenuItem("Back", rectangle, 2, false);
            backItem.ItemTextSize = SpaceGame.GetScale();

            backItem.MouseClicked += new EventHandler<MouseClickEntityEventArgs>(item_MouseJustClicked);
            backItem.MouseLeave += new EventHandler<MouseLeaveEntityEventArgs>(item_MouseLeave);
            backItem.MouseOver += new EventHandler<MouseOverEntityEventArgs>(item_MouseOver);
            Add(backItem);

            items.Add(backItem);
        }

        #region Events

        protected override void item_MouseJustClicked(object sender, MouseClickEntityEventArgs e)
        {
            MenuItem item = (sender as MenuItem);

            if (item != null)
            {
                if (item.ItemPosition == 2)
                    YnG.SwitchState(new SelectShipState());
                else
                {
                    _selectedLevel = item.ItemPosition;
                    Registry.AudioManager.StopMusic();
                    YnG.SwitchState(new SoloPlayerState((SpaceShipType)_selectedShip, (LevelType)_selectedLevel));
                }
            }
        }

        #endregion

        public override void Initialize()
        {
            _title.Position = new Vector2(50, 25);

            spaceLevelImage.Scale = SpaceGame.GetScale();
            spaceLevelImage.Position = new Vector2(SpaceGame.GetScaleX(200), SpaceGame.GetScaleY(130));

            escapeLevelImage.Scale = SpaceGame.GetScale();
            escapeLevelImage.Position = new Vector2(YnG.Width - SpaceGame.GetScaleX(200) - escapeLevelImage.Width, SpaceGame.GetScaleY(130));

            string[] itemNames = { "Deep Space", "Escape" };

            int x = (int)(spaceLevelImage.X + spaceLevelImage.Width / 2) - (MenuItem.BigItemWidth / 2);
            int y = (int)escapeLevelImage.Y + escapeLevelImage.Height + 20;

            for (int i = 0; i < 2; i++)
            {
                if (i > 0)
                    x = (int)(escapeLevelImage.X + escapeLevelImage.Width / 2) - (MenuItem.BigItemWidth / 2);

                MenuItem item = new MenuItem(itemNames[i], new Rectangle(x, y, MenuItem.BigItemWidth, MenuItem.NormalItemHeight), i, i == 0 ? true : false);
                item.ItemTextSize = SpaceGame.GetScale();

                item.MouseClicked += new EventHandler<MouseClickEntityEventArgs>(item_MouseJustClicked);
                item.MouseLeave += new EventHandler<MouseLeaveEntityEventArgs>(item_MouseLeave);
                item.MouseOver += new EventHandler<MouseOverEntityEventArgs>(item_MouseOver);
                Add(item);

                items.Add(item);
            }

            base.Initialize();

            Registry.AudioManager.SpeakAsync("Select a level");
        }

        private void DesactiveAllShipProfile()
        {
            
        }
    }
}
