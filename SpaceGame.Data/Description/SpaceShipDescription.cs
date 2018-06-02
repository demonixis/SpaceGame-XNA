using System;

namespace SpaceGame.Data.Description
{
    public enum SpaceShipType
    {
        ShipA, ShipB, MercsA, MercsB
    }

    public struct ShipSpeed
    {
        public float Left;
        public float Right;
        public float Up;
        public float Down;

        public ShipSpeed(float left, float right, float up, float down)
        {
            Left = left;
            Right = right;
            Up = up;
            Down = down;
        }
    }

    public struct SpaceShipDescription
    {
        public int            Id                 { get; set; }
        public SpaceShipType  SpaceShitType      { get; set; }
        public string         AssetName          { get; set; }
        public float          Health             { get; set; }
        public float          Shield             { get; set; }
        public ShipSpeed      ShipSpeed          { get; set; }
        public int            PrimaryWeaponId    { get; set; }
        public int            SecondaryWeaponId  { get; set; }
        public string         Model              { get; set; }
        public string         Category           { get; set; }
        public int            Weight             { get; set; }
        public string         PrimaryWeapons     { get; set; }
        public string         SecondaryWeapons   { get; set; }
        public string         Description        { get; set; }

        public string GetSpeed()
        {
            float speed = (ShipSpeed.Up + ShipSpeed.Down + ShipSpeed.Left + ShipSpeed.Right) * 1000;
            return speed.ToString();
        }
    }
}
