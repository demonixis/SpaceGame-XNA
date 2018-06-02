using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine;
using Yna.Engine.Graphics;

namespace SpaceGame.UI.Message
{
    public enum MessageBoxType
    {
        System = 0, Start, Pause, End
    }

    public class MessageBoxManager : YnGroup
    {
        private MessageBox _messageBoxSystem;
        private MessageBox _messageBoxStart;
        private MessageBox _messageBoxPause;
        private MessageBox _messageBoxEnd;

        public MessageBox MessageBoxSystem
        {
            get { return _messageBoxSystem; }
        }

        public MessageBox MessageBoxStart
        {
            get { return _messageBoxStart; }
        }

        public MessageBox MessageBoxPause
        {
            get { return _messageBoxPause; }
        }

        public MessageBox MessageBoxEnd
        {
            get { return _messageBoxEnd; }
        }

        public MessageBoxManager()
        {
            _messageBoxSystem = new MessageBox(300, 100);
            _messageBoxStart = new MessageBox(150, 75);
            _messageBoxPause = new MessageBox(150, 90);
            _messageBoxEnd = new MessageBox(250, 150);

            InitializeMessageBoxes();

            DesactiveMessageBoxes();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private void InitializeMessageBoxes()
        {
            _messageBoxSystem.CreateTitle("System");
            _messageBoxSystem.CreateMessage("Message system");
            Add(_messageBoxSystem);

            _messageBoxStart.CreateTitle("Mission 01");
            _messageBoxStart.CreateMessage("Destroy aliens !");
            Add(_messageBoxStart);

            _messageBoxPause.CreateTitle("Pause");
            _messageBoxPause.CreateMessage("Escape to back in menu\n\nP for resume the game");
            Add(_messageBoxPause);

            _messageBoxEnd.CreateTitle("Mission fail...");
            _messageBoxEnd.CreateMessage("You've failed and your ship has destroyed");
            Add(_messageBoxEnd);
        }

        public void DesactiveMessageBoxes()
        {
            for (int i = 0; i < 4; i++)
            {
                Members[i].Active = false;
            }
        }


        public void ActiveMessageBox(MessageBoxType type)
        {
            DesactiveMessageBoxes();

            if (type == MessageBoxType.End)
                _messageBoxEnd.Active = true;
            else if (type == MessageBoxType.Pause)
                _messageBoxPause.Active = true;
            else if (type == MessageBoxType.Start)
                _messageBoxStart.Active = true;
            else if (type == MessageBoxType.System)
                _messageBoxSystem.Active = true;
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < 4; i++)
            {
                if (Members[i].Enabled)
                    Members[i].Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 4; i++)
            {
                if (Members[i].Visible)
                    Members[i].Draw(gameTime, spriteBatch);
            }
        }
    }
}
