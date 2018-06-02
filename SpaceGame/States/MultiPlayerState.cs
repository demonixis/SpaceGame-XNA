using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna.Engine;
using Yna.Engine.Graphics;
using SpaceGame.Manager;
using SpaceGame.UI;
using SpaceGame.UI.Message;
using SpaceGame.Player;
using SpaceGame.Level;
using SpaceGame.Ennemies;
using SpaceGame.Data.Description;

namespace SpaceGame.States
{
    public class MultiPlayerState : YnState2D
    {
        public MultiPlayerState(int nbPlayer)
            : base("multistate", true)
        {


        }
    }
}

