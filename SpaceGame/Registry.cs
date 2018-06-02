using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yna.Engine;
using Yna.Engine.Graphics;
using SpaceGame.Level;
using SpaceGame.Level.Scrolling;
using SpaceGame.Ennemies;
using SpaceGame.Player;
using SpaceGame.Manager;
using SpaceGame.States;
using SpaceGame.UI;
using SpaceGame.UI.Message;
using SpaceGame.Data;
using SpaceGame.Data.Collection;
using SpaceGame.Data.Description;

namespace SpaceGame
{
    public class Registry
    {
        public static EnnemyManager      Ennemies           { get; set; }
        public static SpacePlayer []     Players            { get; set; }
        public static GameUI             GameUI             { get; set; }
        public static MessageBoxManager  MessageBoxManager  { get; set; }
        public static BaseScrolling      Background         { get; set; }
        public static GameStatus         GameStatus         { get; set; }
        public static FxManager          FX                 { get; set; }
        public static YnState2D            ActiveState        { get; set; }
        public static ScoreManager       ScoreManager       { get; set; }
        public static StorageManager     StorageManager     { get; set; }
        public static AudioManager       AudioManager       { get; set; }

        // Base de données
        public static SpaceCollection<EnnemyDescription>     AlienDescriptions      { get; set; }
        public static SpaceCollection<WeaponDescription>     WeaponDescriptions     { get; set; }
        public static SpaceCollection<SpaceShipDescription>  SpaceShipDescriptions  { get; set; }
        public static SpaceCollection<LevelDescription>      LevelDescriptions      { get; set; }

        public static void Clear()
        {
            Ennemies = null;
            Players = null;
            GameUI = null;
            MessageBoxManager = null;
            Background = null;
            ActiveState = null;
        }
    }
}
