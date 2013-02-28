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
        public MiTileEngine TileEngine { get { return tileEngine; } }
        private Song theme;

        protected override void Initialize()
        {
            // Set the game resolution
            MiResolution.SetVirtualResolution(1280, 800);
            MiResolution.SetResolution(900, 600, false);

            // Initialize Input Handler
            inputHandler = new InputHandler(this);

            // Initialize Tile Engine
            tileEngine = new MiTileEngine(this, 100, 100);
            tileEngine.AddTileType('g', "Town View\\Carpet", true);
            tileEngine.AddTileType('r', "Town View\\TownFloor", false);
            tileEngine.AddTileType('t', "Town View\\Carpet_WS", false);
            tileEngine.AddTileType('w', "Town View\\E_Carpet", false);
            tileEngine.AddTileType('q', "Town View\\Carpet_WA", false);
            tileEngine.AddTileType('h', "Town View\\TownFloor_v2", true);

            // Initialize Location Data
            LocationData.Init();

            // Initialize screens
            StartScreen.Instance = new StartScreen(this);
            WorldScreen.Instance = new WorldScreen(this);
            TownScreen.Instance = new TownScreen(this, tileEngine);
            BattleScreen.Instance = new BattleScreen(this, tileEngine);
            MessageScreen.Instance = new MessageScreen(this);
            ChoiceScreen.Instance = new ChoiceScreen(this);

            // Initialize Scripts
            Scripts.Init(this);
            AI.Init(this);

            // Set active screen
            StartScreen.Instance.Activate();

            // Run Tutorial
            ScriptEngine.ExecuteScript(Scripts.Tutorial);

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

            theme = Content.Load<Song>("Sounds\\theme");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(theme);
            base.LoadContent();
        }
    }
}
