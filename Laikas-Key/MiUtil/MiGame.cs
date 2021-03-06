﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MiUtil
{
    public abstract class MiGame : Game
    {
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;
        public SpriteBatch SpriteBatch { get { return spriteBatch; } }
        
        private LinkedList<MiScreen> toDraw;
        private Stack<MiScreen> toUpdate;
        private Queue<MiScreen> updateQueue;

        protected MiInputHandler inputHandler;
        public MiInputHandler InputHandler { get { return inputHandler; } }

        private MiScriptEngine scriptEngine;
        public MiScriptEngine ScriptEngine { get { return scriptEngine; } }

#if DEBUG
        private int frameCounter;
        private int frameRate;
        private TimeSpan elapsedTime = TimeSpan.Zero;
#endif

        public MiGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            MiResolution.Init(ref graphics);

            Content.RootDirectory = "Content";

            toDraw = new LinkedList<MiScreen>();
            toUpdate = new Stack<MiScreen>();
            updateQueue = new Queue<MiScreen>();

            scriptEngine = new MiScriptEngine(this);
        }

        protected override void Initialize()
        {
            if(inputHandler == null)
                throw new InvalidOperationException("Input Handler Not Initialized");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            scriptEngine.Update(gameTime);

            inputHandler.Update(gameTime);

            updateQueue.Clear();
            foreach (MiScreen screen in toUpdate)
                updateQueue.Enqueue(screen);

            foreach (MiScreen screen in updateQueue)
                screen.Update(gameTime);

#if DEBUG
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime <= TimeSpan.FromSeconds(1)) return;

            elapsedTime -= TimeSpan.FromSeconds(1);
            frameRate = frameCounter;
            frameCounter = 0;
#endif

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            MiResolution.BeginDraw();

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, MiResolution.GetTransformationMatrix());

            foreach (MiScreen screen in toDraw)
                screen.Draw(gameTime);
#if DEBUG
            frameCounter++;
            spriteBatch.DrawString(Content.Load<SpriteFont>("Fonts\\Default"), "Frame Rate: " + frameRate + "fps", new Vector2(5, MiResolution.VirtualHeight - 25), Color.White);
#endif

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        public void PushScreen(MiScreen screen)
        {
            toUpdate.Push(screen);
            toDraw.AddLast(screen);
            inputHandler.Focused = screen;
        }

        public void PopScreen()
        {
            toUpdate.Pop();
            toDraw.RemoveLast();
            inputHandler.Focused = toUpdate.Peek();
        }

        public bool ContainsScreen(MiScreen screen)
        {
            return toUpdate.Contains(screen);
        }

        public void RemoveAllScreens()
        {
            toUpdate.Clear();
            toDraw.Clear();
        }
    }
}
