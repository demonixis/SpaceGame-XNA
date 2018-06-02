using System;

namespace SpaceGame.Data
{
    public enum InputType
    {
        Keyboard = 0, Gamepad, Kinect
    }

#if !MONOGAME
    [Serializable]
#endif
    public class GameConfiguration
    {
        public int        Width           { get; set; }
        public int        Height          { get; set; }
        public bool       Sound           { get; set; }
        public bool       Music           { get; set; }
        public bool       Fullscreen      { get; set; }
        public float      SoundVolume     { get; set; }
        public float      MusicVolume     { get; set; }
        public InputType  PreferredInput  { get; set; }

        public GameConfiguration()
        {
            Width = 1280;
            Height = 800;
            Sound = true;
            Music = true;
            Fullscreen = false;
            SoundVolume = 1.0f;
            MusicVolume = 1.0f;
            PreferredInput = InputType.Gamepad;
        }
    }
}
