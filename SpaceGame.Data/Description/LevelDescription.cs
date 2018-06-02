using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.Data.Description
{
    public enum LevelType
    {
        Space = 0, Interior
    }

    public class LevelDescription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EnnemyType[] AuthorizedEnnemy { get; set; }
        public int LevelTimeMax { get; set; }
    }
}
