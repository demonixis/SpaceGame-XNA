using System;

namespace SpaceGame.Ennemies
{
    public class EnnemyDeadEventArgs : EventArgs
    {
        public Ennemy Ennemy { get; set; }

        public EnnemyDeadEventArgs(Ennemy ennemy)
        {
            Ennemy = ennemy;
        }
    }
}
