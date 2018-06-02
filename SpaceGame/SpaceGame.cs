using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine;
using SpaceGame.Manager;
using SpaceGame.States.Loading;
using SpaceGame.Data.Description;
using SpaceGame.Data.Collection;

namespace SpaceGame
{
    public class SpaceGame : YnGame
    {
        #region Helpers de mise à l'échelle

        public const int ScreenWidthReference = 1280;
        public const int ScreenHeightReference = 720;

        public static float GetScaleX(float value) => (((float)YnG.Width * value) / (float)ScreenWidthReference);
        public static float GetScaleY(float value) => (((float)YnG.Height * value) / (float)ScreenHeightReference);


        public static Vector2 GetScale()
        {
            return new Vector2(
                (float)((float)YnG.Width / (float)ScreenWidthReference),
                (float)((float)YnG.Height / (float)ScreenHeightReference));
        }

        #endregion

        public SpaceGame()
            : base(ScreenWidthReference, ScreenHeightReference, String.Format("{0} - v{1}", "Space Game", "1.5a"))
        {
            Registry.StorageManager = new StorageManager();
            Registry.ScoreManager = new ScoreManager();
            Registry.AudioManager = new AudioManager();
            PreloadAssets();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            Content.Dispose();
            Registry.AudioManager.Dispose();
        }

        protected override void Initialize()
        {
            base.Initialize();
            YnG.SwitchState(new SplashscreenState());
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (YnG.Keys.JustPressed(Keys.F5))
                graphics.ToggleFullScreen();
        }

        private void PreloadAssets()
        {
            Registry.ScoreManager.Load();
            Registry.AudioManager.Initialize();
            Registry.AlienDescriptions = Content.Load<SpaceCollection<EnnemyDescription>>("Datas/enemies");
            Registry.SpaceShipDescriptions = Content.Load<SpaceCollection<SpaceShipDescription>>("Datas/players");
            Registry.WeaponDescriptions = Content.Load<SpaceCollection<WeaponDescription>>("Datas/weapons");
        }

        static void Main()
        {
            using (SpaceGame game = new SpaceGame())
                game.Run();
        }
    }
}

