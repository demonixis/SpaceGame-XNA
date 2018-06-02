using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.Data.Description
{
    public enum EnnemyType
    {
        OrganicsA = 0, OrganicsB, RobotA, RobotB, AncienA, AncienB
    }

    public class EnnemyDescription
    {
        public int     Id              { get; set; }
        public string  AssetName       { get; set; }
        public float   Health          { get; set; }
        public float   Shield          { get; set; }
        public int     Framerate       { get; set; }
        public float   Speed           { get; set; }
        public int[]   AnimationSize   { get; set; }
        public int[]   AnimationIndex  { get; set; }
    }
}
