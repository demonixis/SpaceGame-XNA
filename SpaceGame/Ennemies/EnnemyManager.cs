using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Yna.Engine;
using Yna.Engine.Graphics;
using SpaceGame.States;
using SpaceGame.Ennemies;
using SpaceGame.Data.Description;

namespace SpaceGame.Ennemies
{
    public class EnnemyManager : YnGroup
    {
        private YnTimer spawnTimer;
        private Stack<int>[] _recycled;

        private Rectangle PlayableViewport
        {
            get
            {
                if (Registry.Background != null)
                    return Registry.Background.PlayableViewport;
                else
                    return new Rectangle(0, 0, YnG.Width, YnG.Height);
            }
        }

        public event EventHandler<EnnemyDeadEventArgs> EnnemyDied = null;

        private void EnnemyIsDead(EnnemyDeadEventArgs e)
        {
            if (EnnemyDied != null)
                EnnemyDied(this, e);
        }

        public EnnemyManager()
        {
            spawnTimer = new YnTimer(2300);
            spawnTimer.Completed += spawnTimer_ReStarted;

            int numberOfEnnemies = Enum.GetValues(typeof(EnnemyType)).Length;
            _recycled = new Stack<int>[numberOfEnnemies];

            for (int i = 0; i < numberOfEnnemies; i++)
                _recycled[i] = new Stack<int>();
        }

        public override void Revive()
        {
            base.Revive();

            for (int i = 0; i < _recycled.Length; i++)
                _recycled[i].Clear();
            
            Clear();

            spawnTimer.Start();
        }

        void spawnTimer_ReStarted()
        {
            Random random = new Random(DateTime.Now.Millisecond);
            int weaveType = random.Next(0, 3);
            int weaveSize = random.Next(2, 8);

            if (weaveType == 1)
                AddWeaveTypeA(weaveSize);
            else
                AddWeaveTypeB(weaveSize);

            spawnTimer.Restart();
        }

        void ennemy_Killed(object sender, EventArgs e)
        {
            Ennemy ennemy = sender as Ennemy;
            EnnemyIsDead(new EnnemyDeadEventArgs(ennemy));

            _recycled[(int)ennemy.EnnemyType].Push(Members.IndexOf(ennemy));
        }

        private void AddWeaveTypeA(int numSpawn)
        {
            Random rand = new Random(DateTime.Now.Millisecond);

            Ennemy ennemy;

            int randomInt = rand.Next(0, 6);

            // Position de départ de l'ennemie
            // Prise en compte du radius
            int lastPosX = rand.Next(PlayableViewport.X + 100, PlayableViewport.Width / 2);

            for (int i = 0; i < numSpawn; i++)
            {
                if (_recycled[randomInt].Count > 0)
                {
                    ennemy = Members.ElementAt(_recycled[randomInt].Pop()) as Ennemy;
                    ennemy.Revive();
                }
                else
                {
                    ennemy = new Ennemy(Registry.AlienDescriptions[randomInt]);
                    ennemy.LoadContent();
                    ennemy.Killed += new EventHandler<EventArgs>(ennemy_Killed);
                }

                int x = lastPosX + ennemy.Width + 15;
                int y = i * (ennemy.Height - 5);

                ennemy.Initialize(new Vector2(x, y));

                lastPosX = (int)ennemy.X;

                Add(ennemy);
            }
        }

        private void AddWeaveTypeB(int numSpawn)
        {
            Random rand = new Random(DateTime.Now.Millisecond);

            Ennemy ennemy;

            int randomInt = rand.Next(0, 6);

            // Position de départ de l'ennemie
            int lastPosX = rand.Next(PlayableViewport.X + 100, PlayableViewport.Width / 2);

            for (int i = 0; i < numSpawn; i++)
            {
                ennemy = new Ennemy(Registry.AlienDescriptions[randomInt]);

                ennemy.LoadContent();

                int x = lastPosX + ennemy.Width + 15;
                int y = 0;

                if (i % 2 == 0)
                    y -= ennemy.Height - 5;

                ennemy.Initialize(new Vector2(x, y));
                ennemy.Killed += new EventHandler<EventArgs>(ennemy_Killed);

                lastPosX = (int)ennemy.X;

                Add(ennemy);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            spawnTimer.Update(gameTime);
        }
    }
}
