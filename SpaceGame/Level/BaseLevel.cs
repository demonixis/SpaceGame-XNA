using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.State;
using SpaceGame.Ennemies;
using SpaceGame.Manager;
using SpaceGame.Player;
using SpaceGame.Level.Scrolling;
using SpaceGame.Data;
using SpaceGame.Data.Collection;
using SpaceGame.Data.Description;

namespace SpaceGame.Level
{
    public class BaseLevel : YnState2D
    {
        protected YnTimer _timerEndLevel;

        protected SpacePlayer[] _players;
        protected int _nbPlayer;
        protected BaseScrolling[] _scrollings;
        protected EnnemyManager _enemyManager;
        protected SpaceShipDescription[] _spaceShipDescriptions;
        protected LevelDescription _levelDescription;

        public BaseLevel(SpaceShipDescription [] spaceShipDescription, LevelDescription levelDescription)
            : base ("level", true)
        {
            Registry.Clear();

            _nbPlayer = spaceShipDescription.Length;
            _players = new SpacePlayer[_nbPlayer];

            _spaceShipDescriptions = spaceShipDescription;

            for (int i = 0; i < _nbPlayer; i++)
            {
                _players[i] = new SpacePlayer((PlayerIndex)i, _spaceShipDescriptions[i]);
                _players[i].Killed += new EventHandler<EventArgs>(BaseLevel_Killed);
                _players[i].PlayerReallyDead += new EventHandler<EventArgs>(BaseLevel_PlayerReallyDead);
            }
            Registry.Players = _players;

            _enemyManager = new EnnemyManager();
            Registry.Ennemies = _enemyManager;

            _levelDescription = levelDescription;

            _timerEndLevel = new YnTimer(_levelDescription.LevelTimeMax, 0);
            _timerEndLevel.Completed += _timerEndLevel_Completed;
        }

        protected void _timerEndLevel_Completed()
        {
            
        }

        protected void BaseLevel_PlayerReallyDead(object sender, EventArgs e)
        {

        }

        protected void BaseLevel_Killed(object sender, EventArgs e)
        {

        }
    }
}
