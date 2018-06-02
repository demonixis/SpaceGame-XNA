using System;
using Microsoft.Xna.Framework;

namespace SpaceGame.Data.Description
{
    public enum WeaponType
    {
        Laser = 0, Sphere, Burst, Missile
    }

    public struct WeaponDescription
    {
        public int          Id                { get; set; }
        public WeaponType   WeaponType        { get; set; }
        public string       AssetName         { get; set; }
        public int          Damage            { get; set; }
        public int          Interval          { get; set; }
        public int          Speed             { get; set; }
        public Vector2      Acceleration      { get; set; }
        public Vector2[]    Directions        { get; set; }
        public float[]      Rotations         { get; set; }
        public string       SoundAsset        { get; set; }
    }
}
