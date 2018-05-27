using System;
using System.IO;
using ClEngine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using NLua;

namespace DirectxGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Random random;
        private Lua _lua;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            random = new Random();
            _lua = new Lua
            {
                ["print"] = new Action<object>(Print),
            };
            _lua.LoadCLRPackage();

            Exiting += (sender, args) => IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _lua.NewTable("system");
            _lua["system.graphics"] = graphics;
            _lua["system.window"] = Window;
            _lua.RegisterFunction("system.random", this, GetType().GetMethod("Random"));

            _lua.NewTable("camera");
            _lua.RegisterFunction("camera.create", this, GetType().GetMethod("CreateCamera"));

            _lua.NewTable("content");
            _lua.RegisterFunction("content.load", this, GetType().GetMethod("LoadContent"));

            _lua.NewTable("spritebatch");
            _lua.RegisterFunction("spritebatch.create", this, GetType().GetMethod("CreateSpriteBatch"));

            _lua.DoFile(Path.Combine("scripts", "init.lua"));
            (_lua["Main"] as LuaFunction)?.Call();

            base.Initialize();
        }

        public int Random(int min, int max)
        {
            return random.Next(min, max);
        }

        public Camera2D CreateCamera()
        {
            return new Camera2D(GraphicsDevice);
        }

        public SpriteBatch CreateSpriteBatch()
        {
            return new SpriteBatch(GraphicsDevice);
        }

        public object LoadContent(string name)
        {
            return Content.Load<object>(name);
        }

        private void Print(object message)
        {
            MessageCache.Messages.Add(new MessageModel { Message = message });
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
