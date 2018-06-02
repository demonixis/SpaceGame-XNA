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
    public abstract class BaseMenu : YnState2D
    {
        // Temps d'activiation d'un item quand le pointer est dessus (Kinect)
        private const int KinectTimerDuration = 4000;

        protected YnText _title;                        // Titre
        protected YnEntity _handSelector;                // Curseur de souris/Kinect
        protected List<MenuItem> items;                 // Elements du menu

#if COMPLETE
        protected KinectSensorController kinect;        // Gestionnaire Kinect
        protected Vector3 _lastKinectPosition;          // Dernière position connue du pointeur géré par Kinect
        protected YnTimer _timerKinectOver;             // Timer permettant de valider un élements du menu
        protected YnTransition _transitionKinectOver;   // Transition sur le curseur kinect quand il est en mode "selection active"
        protected bool _kinectValidedAction;            // Indique si l'item a été validé après le temps d'activation
#endif
        /// <summary>
        /// Représente un écran de menu standard avec un titre et des items
        /// </summary>
        public BaseMenu(string title, int numItems)
            : base("basemenu", true)
        {
            // 1 - Le titre
            _title = new YnText("Fonts/Menu", title);
            _title.Color = Color.GhostWhite;
            _title.Scale = new Vector2(1.5f, 1.5f);
            Add(_title);

            string pointerAssetName = Assets.MouseCursor;

#if COMPLETE
            kinect = KinectSensorController.Instance;

            _lastKinectPosition = Vector3.Zero;

            // Aucune action validée pour le moment
            _kinectValidedAction = false;

            // Evenements kinect
            _timerKinectOver = new YnTimer(KinectTimerDuration, 0);
            _timerKinectOver.Completed += new EventHandler<EventArgs>(_timerKinectOver_Completed);
            _timerKinectOver.ReStarted += new EventHandler<EventArgs>(_timerKinectOver_ReStarted);

            _transitionKinectOver = new YnTransition(KinectTimerDuration);

            if (kinect.IsAvailable)
                pointerAssetName = Assets.KinectCursor;
#endif

            items = new List<MenuItem>(numItems);

            // Le curseur de souris/Kinect
            _handSelector = new YnEntity(pointerAssetName);
        }

#if COMPLETE

        #region Evenements Kinect

        protected virtual void _timerKinectOver_ReStarted(object sender, EventArgs e)
        {
            _transitionKinectOver.StartFadeOut();
            _handSelector.Color = Color.GreenYellow;
        }

        protected virtual void _timerKinectOver_Completed(object sender, EventArgs e)
        {
            _handSelector.Color = Color.White;
            _handSelector.Alpha = 1.0f;
            _kinectValidedAction = true;
        }

        #endregion

#endif

        #region Evenements souris

        // Lorsque la souris est au dessus d'un item du menu
        protected virtual void item_MouseOver(object sender, MouseOverEntityEventArgs e)
        {
            MenuItem item = (sender as MenuItem);

            if (item != null)
            {
                if (!item.Selected)
                {
                    DeselectAllItems(item);
                    item.SetSelected(true);
                }
            } 
        }

        // Lorsque la souris n'est plus sur un item du menu
        protected virtual void item_MouseLeave(object sender, MouseLeaveEntityEventArgs e)
        {
            MenuItem item = (sender as MenuItem);

            item.SetSelected(false);

            if (item != null)
                DeselectAllItems();
        }

        // Lorsqu'on click sur un item du menu
        protected abstract void item_MouseJustClicked(object sender, MouseClickEntityEventArgs e);

        #endregion

        #region GameState pattern

        public override void Initialize()
        {
            base.Initialize();

            // Le curseur doit être au premier plan (donc ajouté en dernier après la construction)
            
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Add(_handSelector);
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

#if COMPLETE
            if (kinect.IsAvailable)
            {
                _timerKinectOver.Update(gameTime);

                Vector3 hand = kinect.GetUserProfile(KinectPlayerIndex.One).HandRight;
                _handSelector.Position = new Vector2(hand.X, hand.Y);

                foreach (MenuItem item in items)
                {
                    if (CheckKinectOver(item, (int)hand.X, (int)hand.Y))
                    {
                        item_MouseOver(item, new MouseOverSpriteEventArgs((int)hand.X, (int)hand.Y));

                        if (_kinectValidedAction)
                            item_MouseJustClicked(item, new MouseClickSpriteEventArgs((int)hand.X, (int)hand.Y, Yna.Input.MouseButton.Left, true, false));

                        if (_timerKinectOver.Active)
                        {
                            _transitionKinectOver.Update(gameTime);
                            _handSelector.Alpha = _transitionKinectOver.Alpha;
                        }
                        else
                        {
                            _timerKinectOver.Restart();
                        }
                    }

                    else if (CheckKinectLeave(item, (int)_lastKinectPosition.X, (int)_lastKinectPosition.Y, (int)hand.X, (int)hand.Y))
                    {
                        item_MouseLeave(item, new MouseLeaveSpriteEventArgs((int)_lastKinectPosition.X, (int)_lastKinectPosition.Y, (int)hand.X, (int)hand.Y));
                        _timerKinectOver.Stop();
                        _handSelector.Color = Color.White;
                        _handSelector.Alpha = 1.0f;
                        _kinectValidedAction = false;
                    }
                }

                _lastKinectPosition = hand;
            }
            else
                _handSelector.Position = new Vector2(YnG.Mouse.X, YnG.Mouse.Y);
#else
            _handSelector.Position = new Vector2(YnG.Mouse.X, YnG.Mouse.Y);
#endif
        }

        #endregion

        // Déselectionne tous les items du menu
        private void DeselectAllItems(MenuItem excludedItem = null)
        {
            foreach (MenuItem item in items)
            {
                if (excludedItem == null)
                    item.Selected = false;

                else if (excludedItem != item)
                    item.Selected = false;
            }
        }

        #region Detection de collisions sur les items du menu avec Kinect

        // Indique si le pointeur dirigé par Kinect est sur un item du menu
        protected bool CheckKinectOver(MenuItem item, int x, int y)
        {
            return item.Rectangle.Contains(x, y);
        }

        // Indique si le pointeur dirigé par Kinect était sur un item du menu et qu'il est partie
        protected bool CheckKinectLeave(MenuItem item, int lastX, int lastY, int x, int y)
        {
            return CheckKinectOver(item, lastX, lastY);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        #endregion
    }
}
