using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using SpaceGame.Data;

namespace SpaceGame.Manager
{
#if !MONOGAME
    [Serializable]
#endif
    public class ScoreManager
    {
        private const int MaxStoredScores = 10;

        private GameScore _currentScore;
        private List<GameScore> _scores;

        public GameScore CurrentGameScore
        {
            get { return _currentScore; }
            set { _currentScore = value; }
        }

        public List<GameScore> GameScores
        {
            get { return _scores; }
            set { _scores = value; }
        }

        public GameScore this[int index]
        {
            get
            {
                if (index < 0 || index > _scores.Count - 1)
                    return null;
                else
                    return _scores[index];
            }
            set
            {
                if (index < 0 || index > _scores.Count - 1)
                    throw new IndexOutOfRangeException();
                else
                    _scores[index] = value;
            }
        }

        public ScoreManager()
        {
            _scores = new List<GameScore>(MaxStoredScores);

            _currentScore = null;
        }

        public void Add(GameScore score)
        {
            if (_scores.Count == MaxStoredScores)
                _scores.RemoveAt(0);

            _scores.Add(score);
        }

        public void Update()
        {
            Add(_currentScore);
        }

        public void Record()
        {
            Registry.StorageManager.RecordGameSave();
        }

        public void Load()
        {
            Registry.StorageManager.LoadGameSave();
        }
    }
}
