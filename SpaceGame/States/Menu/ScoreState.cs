using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Animation;
using Yna.Engine.State;
using Yna.Engine.Graphics.Event;
#if COMPLETE
using Yna.Input.Kinect;
#endif
using SpaceGame.Manager;
using SpaceGame.UI;
using SpaceGame.UI.Menu;
using SpaceGame.Data;
using SpaceGame.Data.Description;

namespace SpaceGame.States.Menu
{
    public class ScoreState : BaseMenu
    {
        YnEntity background;

        ScoreManager _playerScore = Registry.ScoreManager;

        public ScoreState(bool saveScore = false)
            : base("Scores", 1)
        {
            //background = new YnEntity("Backgrounds/options");
            //Add(background);

            int scoreSize = Registry.ScoreManager.GameScores.Count;
            int x = (int)SpaceGame.GetScaleX(150);

            for (int i = 0; i < scoreSize; i++)
            {
                YnText text = new YnText("Fonts/Menu", String.Format("{0}", _playerScore[i].GetDetailedMenuString(7, 7)), new Vector2(x, 130 + i * 55), Color.White);
                text.Scale = new Vector2(1.2f) * SpaceGame.GetScale();
                Add(text);
            }

            Rectangle rectangle = new Rectangle(
                YnG.Width / 2 - (int)SpaceGame.GetScaleX(MenuItem.SmallItemWidth) / 2,
                YnG.Height - (int)SpaceGame.GetScaleY(75),
                (int)SpaceGame.GetScaleX(MenuItem.SmallItemWidth),
                (int)SpaceGame.GetScaleY(MenuItem.SmallItemHeight));

            MenuItem item = new MenuItem("Menu", rectangle, 0, false);
            item.ItemTextSize = SpaceGame.GetScale();
            item.MouseClicked += new EventHandler<MouseClickEntityEventArgs>(item_MouseJustClicked);
            item.MouseLeave += new EventHandler<MouseLeaveEntityEventArgs>(item_MouseLeave);
            item.MouseOver += new EventHandler<MouseOverEntityEventArgs>(item_MouseOver);
            Add(item);

            items.Add(item);

            if (saveScore)
            {
                _playerScore.Update();
                _playerScore.Record();
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            _title.Position = new Vector2(50, 25);

            Registry.AudioManager.SpeakAsync("Scores");

            //background.Position = new Vector2(YnG.Width / 2 - background.Width / 2, YnG.Height / 2 - background.Height / 2);
        }

        protected override void item_MouseJustClicked(object sender, MouseClickEntityEventArgs e)
        {
            YnG.SwitchState(new MenuState());
        }
    }
}
