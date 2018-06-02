using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine;
using Yna.Engine.Graphics;

namespace SpaceGame.UI.Message
{
    public class MessageBox : YnGroup
    {
        private YnText _title;
        private YnText _message;
        private String _baseMessage;
        private YnSprite _container;

        public string Title
        {
            get
            {
                if (_title != null)
                    return _title.Text;
                else
                    return "";
            }
            set
            {
                if (_title != null)
                {
                    _title.Text = value;

                    _title.Position = new Vector2(
                       (float)(_container.X + _container.Width / 2 - (_title.Width / 2)),
                       (float)(_container.Y + 10));
                }
                else
                    throw new Exception("Title is not instancied yet");
            }
        }

        public string BaseMessage
        {
            get { return _baseMessage; }
        }

        public string Message
        {
            get
            {
                if (_message != null)
                    return _message.Text;
                else
                    return "";
            }
            set
            {
                if (_message != null)
                {
                    _message.Text = value;
                }
                else
                    throw new Exception("Title is not instancied yet");
            }
        }

        public Color TitleColor
        {
            get { return _title.Color; }
            set { _title.Color = value; }
        }

        public Color MessageColor
        {
            get { return _message.Color; }
            set { _message.Color = value; }
        }

        public Color BoxColor
        {
            get { return _container.Color; }
            set { _container.Color = value; }
        }

        public float BoxAlpha
        {
            get { return _container.Alpha; }
            set { _container.Alpha = value; }
        }

        public int BoxWidth
        {
            get { return _container.Width; }
            set { _container.Width = value; }
        }

        public int BoxHeight
        {
            get { return _container.Height; }
            set { _container.Height = value; }
        }

        #region events

        public event EventHandler<EventArgs> CloseRequested = null;

        protected void OnCloseRequest(EventArgs e)
        {
            if (CloseRequested != null)
                CloseRequested(this, e);
        }

        #endregion

        public MessageBox(int width, int height)
        {
            _alpha = 0.6f;

            _rectangle = new Rectangle(0, 0, width, height);

            _container = new YnSprite("UI/MessageBox");
            _container.LoadContent();
    
            Add(_container);

            CenterToScreen(width, height);
        }

        private void CenterToScreen(int width, int height)
        {
            _container.X = YnG.Width / 2 - _container.Width / 2;
            _container.Y = YnG.Height / 2 - _container.Height / 2;
        }

        public void CreateTitle(string message)
        {
            _title = new YnText("Fonts/Titles", message);
            _title.LoadContent();
            _title.Initialize();
            _title.Position = new Vector2(
                (float)(_container.X + _container.Width / 2 - (_title.Width / 2)),
                (float)(_container.Y + 10));
            _title.Color = Color.White;
            Add(_title);
        }

        public void CreateMessage(string message)
        {
            if (_message == null)
            {
                _message = new YnText("Fonts/Messages", message);
                _message.LoadContent();
                _message.Initialize();
                _message.Position = new Vector2(
                    (float)(_container.X + _container.Width / 2 - (_message.Width / 2)),
                    (float)(_container.Y + 65));
                _message.Color = Color.White;
                Add(_message);

                if (_message.Height + 65 > _container.Height)
                    _container.Scale = new Vector2(_container.Scale.X, _container.Scale.Y + (_message.Height / _message.Height + 65.0f) / 100.0f);

                // Message d'origine
                _baseMessage = _message.Text;
            }
            else
                throw new Exception("[MessageBox] Message already exists");
        }
    }
}
