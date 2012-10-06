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
    public class Laikas : MiGame
    {
        protected override void Initialize()
        {
            // Set the game resolution
            MiResolution.SetVirtualResolution(800, 600);
            MiResolution.SetResolution(800, 600, false);

            // Initialize Input Handler
            inputHandler = new InputHandler(this);

            // Initialize screens
            StartScreen.Instance = new StartScreen(this);
            WorldScreen.Instance = new WorldScreen(this);

            // Set active screen
            ToDraw.AddLast(StartScreen.Instance);
            ToUpdate.Push(StartScreen.Instance);

            ScriptEngine.ExecuteScript(new MiScript(StartScreen.Instance.EntrySequence));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            StartScreen.Instance.LoadContent();
            WorldScreen.Instance.LoadContent();
            base.LoadContent();
        }
    }
}
