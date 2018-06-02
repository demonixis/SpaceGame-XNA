using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Animation;

namespace SpaceGame.UI.Menu
{
    public class MenuItem : YnGroup
    {
        public static int BigItemWidth => (int)SpaceGame.GetScaleX(350);
        public static int BigItemHeight => (int)SpaceGame.GetScaleY(75); 
        public static int BigBorderSize => (int)SpaceGame.GetScaleX(5);
        public static int NormalItemWidth => (int)SpaceGame.GetScaleX(150);
        public static int NormalItemHeight => (int)SpaceGame.GetScaleY(65);
        public static int NormalBorderSize => (int)SpaceGame.GetScaleX(3);
        public static int SmallItemWidth => (int)SpaceGame.GetScaleX(120);
        public static int SmallItemHeight => (int)SpaceGame.GetScaleY(45);
        public static int SmallBorderSize => (int)SpaceGame.GetScaleX(2);


        // Border est un rectangle plus gros que le fond
        public Color BorderColor = Color.White;
        public Color BackgroundColor = new Color(0, 108, 165);

        private YnSprite _foreground;
        private YnSprite _background;
        private YnText _contentText;
        private bool _selected;
        private int _itemPosition;

        // Transition
        private YnTransitionEffect _transition;

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        public int ItemPosition => _itemPosition;

        public void SetSelected(bool selected)
        {
            _selected = selected;

            if (_selected)
            {
                _contentText.Color = Color.AntiqueWhite;
                _transition.StartFadeIn();
            }
            else
            {
                _contentText.Color = Color.White;
                _transition.StartFadeOut();
            }
        }

        public Vector2 ItemTextSize
        {
            get { return _contentText.Scale; }
            set { _contentText.Scale = value; }
        }

        public MenuItem(string text, Rectangle rectangle, int position, bool selected = false)
        {
            _itemPosition = position;
            _selected = selected;

            SetRectangle(ref rectangle);

            _background = new YnSprite(Rectangle, BorderColor);
            _background.Alpha = 0.0f;
            Add(_background);

            _foreground = new YnSprite(new Rectangle((int)X, (int)Y, rectangle.Width - BigBorderSize, rectangle.Height - BigBorderSize), BackgroundColor);
            Add(_foreground);

            _contentText = new YnText("Fonts/Menu", text);
            _contentText.Color = Color.White;
            Add(_contentText);

            _transition = new YnTransitionEffect(350.0f, 250.0f);
        }

        public override void Initialize()
        {
            base.Initialize();

            _foreground.Position = new Vector2(
                (int)(_background.X + _background.Width / 2 - _foreground.Width / 2),
                (int)(_background.Y + _background.Height / 2 - _foreground.Height / 2));

            CenterText();
        }

        private void CenterText()
        {
            _contentText.Position = new Vector2(
                (_background.X + _background.Width / 2) - _contentText.Width / 2,
                (_background.Y + _background.Height / 2) - _contentText.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Mise à jour de la transition
            _transition.Update(gameTime);

            if (_transition.TransitionState == TransitionState.FadeIn)
                _background.Alpha = _transition.Alpha;

            else if (_transition.TransitionState == TransitionState.FadeOut)
                _background.Alpha = _transition.Alpha;
        }
    }
}
