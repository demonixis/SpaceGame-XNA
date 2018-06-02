using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Yna.Engine;
using Yna.Engine.Graphics;
using SpaceGame.Player;
using SpaceGame.Data.Description;

namespace SpaceGame.UI.Menu
{
    public class ShipProfileItem : YnGroup
    {
        private const int BorderSize = 2;

        private SpaceShipType _shipType;
        private bool _selected;

        private YnSprite _shipImage;
        private YnSprite _border;
        private YnSprite _container;
        private YnText[] _descriptions;

        private SpaceShipDescription spaceShipDescription;

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        public SpaceShipDescription SpaceShipDescription
        {
            get { return spaceShipDescription; }
        }

        public ShipProfileItem(SpaceShipType type, bool selected = false)
        {
            _shipType = type;
            Selected = selected;

            int widthOverTwo = YnG.Width / 2;
            int heightOverTwo = YnG.Height / 2;

            _border = new YnSprite(new Rectangle(
                (int)SpaceGame.GetScaleX(450),
                (int)SpaceGame.GetScaleY(150), 
                (int)SpaceGame.GetScaleX(650), 
                (int)SpaceGame.GetScaleY(400)),
                Color.White);

            Add(_border);

            _container = new YnSprite(new Rectangle(
                (int)_border.X + BorderSize, 
                (int)_border.Y + BorderSize,
                _border.Width - BorderSize * 2, 
                _border.Height - BorderSize - 2), new Color(0, 108, 165));
            Add(_container);

            _shipImage = new YnSprite();

            spaceShipDescription = Registry.SpaceShipDescriptions[(int)_shipType];
        }

        public override void LoadContent()
        {
            base.LoadContent();

            List<string> descriptionText = new List<string>(7);

            descriptionText.Add(String.Format("Model:              {0}", spaceShipDescription.Model));
            descriptionText.Add(String.Format("Category            {0}", spaceShipDescription.Category));
            descriptionText.Add(String.Format("Weight:             {0}", spaceShipDescription.Weight));
            descriptionText.Add(String.Format("Speed:              {0}", spaceShipDescription.GetSpeed()[0] * spaceShipDescription.GetSpeed()[1]));
            descriptionText.Add(String.Format("Primary weapon:     {0}", spaceShipDescription.PrimaryWeapons));
            descriptionText.Add(String.Format("Secondary weapon:   {0}", spaceShipDescription.SecondaryWeapons));
            descriptionText.Add(String.Format("Ship descrption:\n\n{0}", spaceShipDescription.Description));

            _shipImage.AssetName = spaceShipDescription.AssetName.Split(new char[] { '_' })[0].ToString();
            Add(_shipImage);

            _descriptions = new YnText[descriptionText.Count];
            for (int i = 0; i < descriptionText.Count; i++)
            {
                _descriptions[i] = new YnText("Fonts/Messages", descriptionText[i], new Vector2(_container.X + 20, (_container.Y + 20) + 25 * i), Color.White);
                _descriptions[i].Scale = new Vector2(0.8f) * SpaceGame.GetScale();
                _descriptions[i].Color = Color.White;
                Add(_descriptions[i]);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            Vector2 scale = new Vector2(1.3f) * SpaceGame.GetScale();

            _shipImage.Origin = new Vector2(_shipImage.Width / (2 * scale.X) , _shipImage.Height / (2 * scale.Y));
            _shipImage.Scale = scale;

            _shipImage.Position = new Vector2(
                ((_container.X + _container.Width) - _shipImage.Width / (2 * scale.Y)) - 50,
                _container.Y + _shipImage.Height / (2 * scale.Y));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Enabled)
                _shipImage.Rotation += 0.01f;
        }
    }
}
