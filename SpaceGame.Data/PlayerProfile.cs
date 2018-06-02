using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.Data
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class PlayerProfile
    {
        private GameScore _gameScore;
        private long _totalTimePlayed;
        private uint _nbPlayed;
        private uint _nbWinned;
        private uint _nbLoosed;
        private string _name;

        public GameScore GameScore
        {
            get { return _gameScore; }
            set { _gameScore = value; }
        }

        public long TotalTimePlayed
        {
            get { return _totalTimePlayed; }
            set { _totalTimePlayed = value; }
        }

        public uint NbPlayed
        {
            get { return _nbPlayed; }
            set { _nbPlayed = value; }
        }

        public uint NbWinned
        {
            get { return _nbWinned; }
            set { _nbWinned = value; }
        }

        public uint NbLoosed
        {
            get { return _nbLoosed; }
            set { _nbLoosed = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public PlayerProfile()
        {
            _name = "Player 1";
            _nbPlayed = 0;
            _nbWinned = 0;
            _nbLoosed = 0;
            _totalTimePlayed = 0;
            _gameScore = new GameScore(_name, 0);
        }

        public PlayerProfile(string name, int index)
            : this()
        {
            _name = name;
            _gameScore = new GameScore(_name, index);
        }
    }
}
