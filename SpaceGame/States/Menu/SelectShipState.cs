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
using System.Globalization;
using SpaceGame.Data.Description;

namespace SpaceGame.States.Menu
{
    public class SelectShipState : BaseMenu
    {
        private ShipProfileItem[] shipProfileItem;
        private int _selectedShip;

        private int ItemWidth
        {
            get { return (int)SpaceGame.GetScaleX(200); }
        }

        private int ItemHeight
        {
            get { return (int)SpaceGame.GetScaleY(55); }
        }

        public SelectShipState()
            : base("Space ship selection", 4)
        {
            _selectedShip = 0;

            string[] itemsText = { "ZX Thunder 4", "Pulsar X6", "MRC-A1", "Mantra N4", "Back", "Ready" };

            shipProfileItem = new ShipProfileItem[4];

            for (int i = 0; i < 4; i++)
            {
                int y = (int)SpaceGame.GetScaleY(150) + i * ItemHeight + (int)SpaceGame.GetScaleY(10);

                MenuItem item = new MenuItem(itemsText[i], new Rectangle(
                    (int)SpaceGame.GetScaleX(50), 
                    y, 
                    ItemWidth, 
                    ItemHeight), 
                    i, 
                    i == 0 ? true : false);

                item.ItemTextSize = SpaceGame.GetScale();

                item.ItemTextSize = new Vector2(0.6f) * SpaceGame.GetScale();
                item.MouseOver += item_MouseOver;
                item.MouseLeave += item_MouseLeave;
                item.MouseClicked += item_MouseJustClicked;
                Add(item);

                items.Add(item);

                shipProfileItem[i] = new ShipProfileItem((SpaceShipType)i, i == 0 ? true : false);
                shipProfileItem[i].Active = ((i == 0) ? true : false);
                Add(shipProfileItem[i]);
            }

            for (int j = 4; j < 6; j++)
            {
                int x = (int)SpaceGame.GetScaleX(50);

                if (j > 4)
                    x = YnG.Width - MenuItem.SmallItemWidth - 50;

                int y = YnG.Height - (MenuItem.SmallItemHeight + (MenuItem.SmallItemHeight / 3));

                MenuItem item = new MenuItem(itemsText[j], new Rectangle(x, y, MenuItem.SmallItemWidth, MenuItem.SmallItemHeight), j, false);
                item.ItemTextSize = new Vector2(0.6f) * SpaceGame.GetScale();
                item.MouseOver += item_MouseOver;
                item.MouseLeave += item_MouseLeave;
                item.MouseClicked += item_MouseJustClicked;
                Add(item);

                items.Add(item);
            }
        }

        #region Events

        protected override void item_MouseJustClicked(object sender, MouseClickEntityEventArgs e)
        {
            MenuItem item = (sender as MenuItem);

            if (item != null)
            {
                if (item.ItemPosition < 4)
                {
                    DesactiveAllShipProfile(shipProfileItem[item.ItemPosition]);
                    shipProfileItem[item.ItemPosition].Active = true;
                    _selectedShip = item.ItemPosition;

                    Registry.AudioManager.SpeakAsync(shipProfileItem[item.ItemPosition].SpaceShipDescription.Model + ". " + shipProfileItem[item.ItemPosition].SpaceShipDescription.Description);
                }
                else if (item.ItemPosition == 4)
                    YnG.SwitchState(new MenuState());
                else if (item.ItemPosition == 5)
                    YnG.SwitchState(new SelectLevelState(_selectedShip));
            }
        }

        #endregion

        public override void Initialize()
        {
            base.Initialize();

            _title.Position = new Vector2(50, 25);

            Registry.AudioManager.SpeakAsync("Select a ship");
        }

        private void DesactiveAllShipProfile(ShipProfileItem except = null)
        {
            foreach (ShipProfileItem item in shipProfileItem)
            {
                if (except != null && except != item)
                    item.Active = false;
                else
                    item.Active = false;
            }
        }
    }
}
