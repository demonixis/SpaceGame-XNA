using System;
using Microsoft.Xna.Framework;
using Yna.Engine;
using Yna.Engine.Graphics;
using SpaceGame.Player;
using SpaceGame.Data.Description;

namespace SpaceGame.UI.Player
{
    internal class WeaponItem : YnGroup
    {
        private WeaponType _identifier;
        public YnSprite _selector;
        private YnSprite _image;
        private YnText _label;

        public WeaponType Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        public WeaponItem(int x, int y, string texte, Color backgroundColor, Color foregroundColor, string logoAsset, WeaponType id, bool selected = false)
        {
            X = x;
            Y = y;

            Identifier = id;

            _selector = new YnSprite(new Rectangle((int)X, (int)Y, 130, 20), foregroundColor);
            _selector.Visible = selected;
            _selector.Color *= 0.35f;
            Add(_selector);

            _label = new YnText("Fonts/HUD", texte, new Vector2(X, Y), Color.Yellow);
            _label.Scale = new Vector2(0.75f, 0.75f);
            Add(_label);

            _image = new YnSprite(new Vector2(X, Y), logoAsset);
            _image.Scale = new Vector2(0.4f);
            Add(_image);
        }

        public override void Initialize()
        {
            base.Initialize();
            _selector.Position = new Vector2(X, Y);
            _image.Position = new Vector2(X + _image.Width / 2, Y + _image.Height / 2);
            _image.Origin = new Vector2(_image.Width / 2, _image.Height / 2);
            _label.Position = new Vector2(_selector.X + 40, _selector.Y + _selector.Height / 2 - _label.Height / 3);
        }

        public new Vector2 Position
        {
            get { return _position; }
            set 
            { 
                _position = value;

                foreach (YnEntity entity in this)
                    entity.Position += value;
            }
        }

        public new int Width
        {
            get { return _selector.Width; }
        }

        public new int Height
        {
            get { return _selector.Height; }
        }
    }
}
