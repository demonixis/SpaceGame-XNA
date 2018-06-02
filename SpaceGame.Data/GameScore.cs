using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.Data
{
#if !MONOGAME
    [Serializable]
#endif
    public class GameScore
    {
        public string    PlayerName   { get; set; }
        public int       PlayerIndex  { get; set; }
        public int       Score        { get; set; }
        public int       Level        { get; set; }
        public DateTime  Date         { get; set; }

        public GameScore()
        {
            PlayerName = "Player 1";
            PlayerIndex = 0;
            Score = 0;
            Level = 1;
            Date = new DateTime(DateTime.Now.Millisecond);
        }

        public GameScore(string playerName, int index)
        {
            PlayerName = playerName;
            PlayerIndex = index;
            Score = 0;
            Level = 0;
            Date = new DateTime(DateTime.Now.Millisecond); ;
        }

        public GameScore(string playerName, int index, int score, int level, DateTime date)
            : this (playerName, index)
        {
            Score = score;
            Level = level;
            Date = date;
        }

        public string GetMenuString(int length)
        {
            string score = Score.ToString();
            
            int size = length - score.ToCharArray().Count();

            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                    score = "0" + score;
            }
            return score;
        }

        public string GetDetailedMenuString(int nameLength, int scoreLength)
        {
            string name = PlayerName;
            int size = nameLength - name.ToCharArray().Count();

            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                    name += " ";
            }

            return String.Format("{0}    {1}", name, GetMenuString(scoreLength));
        }
    }
}
