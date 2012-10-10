using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MiUtil;

namespace Laikas_Key
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class Laikas : MiGame
    {
        private MiTileEngine tileEngine;

        protected override void Initialize()
        {
            // Set the game resolution
            MiResolution.SetVirtualResolution(1280, 800);
            MiResolution.SetResolution(900, 600, false);

            // Initialize Input Handler
            inputHandler = new InputHandler(this);

            // Initialize Tile Engine
            tileEngine = new MiTileEngine(this, 75, 75);
            tileEngine.AddTileType('g', "Passable", true);
            tileEngine.AddTileType('r', "Road", false);
            tileEngine.AddTileType('t', "Treasure", false);

            // Initialize screens
            StartScreen.Instance = new StartScreen(this);
            WorldScreen.Instance = new WorldScreen(this);
            TownScreen.Instance = new TownScreen(this, tileEngine);
            BattleScreen.Instance = new BattleScreen(this, tileEngine);
            MessageScreen.Instance = new MessageScreen(this);
            ChoiceScreen.Instance = new ChoiceScreen(this);

            // Set active screen
            PushScreen(StartScreen.Instance);

            ScriptEngine.ExecuteScript(new MiScript(StartScreen.Instance.EntrySequence));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            tileEngine.LoadContent();
            StartScreen.Instance.LoadContent();
            WorldScreen.Instance.LoadContent();
            TownScreen.Instance.LoadContent();
            BattleScreen.Instance.LoadContent();
            MessageScreen.Instance.LoadContent();
            ChoiceScreen.Instance.LoadContent();
            base.LoadContent();
        }
    }
}
