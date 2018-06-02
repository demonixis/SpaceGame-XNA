using System;
using Microsoft.Xna.Framework;
using Yna.Engine;
using Yna.Engine.Graphics;

namespace SpaceGame.Level.Scrolling
{
    public class PlanetsScrolling : YnGroup
    {
        public PlanetsScrolling()
            : base()
        {
            // Charger un schéma de scrolling pour les planètes
        }

        public override void Initialize()
        {
            base.Initialize();

            int offset = 150;

            Add(createPlanet(Assets.PlanetRed, new Vector2(300 / 3, -offset)));
            Add(createPlanet(Assets.PlanetGreen, new Vector2(150, YnG.Height * 4 + offset)));
            Add(createPlanet(Assets.PlanetBlue, new Vector2(800, YnG.Height * 8 + offset)));
        }

        private YnSprite createPlanet(string asset, Vector2 position)
        {
            YnSprite planet = new YnSprite(asset);
            planet.Position = position;
            planet.Viewport = new Rectangle(0, 0, YnG.Width, YnG.Height * 12);
            planet.AllowAcrossScreen = true;
            planet.Velocity = new Vector2(0.1f, 1.0f);
            planet.Scale = new Vector2(1.5f, 1.5f);

            return planet;
        }
    }
}
