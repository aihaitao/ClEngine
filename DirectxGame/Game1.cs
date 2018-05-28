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
        public static Lua Lua;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = Path.Combine(Environment.CurrentDirectory, "Content");

            random = new Random();
            Lua = new Lua
            {
                ["print"] = new Action<object>(Print),
            };
            Lua.LoadCLRPackage();
            Lua.NewTable("system");
            Lua["system.graphics"] = graphics;
            Lua["system.window"] = Window;
            Lua["system.game"] = this;
            Lua.RegisterFunction("system.endgame", this, GetType().GetMethod("EndGame"));
            Lua.RegisterFunction("system.random", this, GetType().GetMethod("Random"));

            Lua.NewTable("camera");
            Lua.RegisterFunction("camera.create", this, GetType().GetMethod("CreateCamera"));

            Lua.NewTable("content");
            Lua.RegisterFunction("content.load", this, GetType().GetMethod("LoadContent"));

            Lua.NewTable("spritebatch");
            Lua.RegisterFunction("spritebatch.create", this, GetType().GetMethod("CreateSpriteBatch"));

            Lua.NewTable("vector2");
            Lua["vector2.one"] = Vector2.One;
            Lua.RegisterFunction("vector2.create", this, GetType().GetMethod("CreateVector2"));

            Lua.NewTable("color");
            Lua["color.white"] = Color.White;
            Lua["color.cornflowerblue"] = Color.CornflowerBlue;
            Lua["color.aliceblue"] = Color.AliceBlue;
            Lua["color.antiquewhite"] = Color.AntiqueWhite;
            Lua["color.aqua"] = Color.Aqua;
            Lua["color.aquamarine"] = Color.Aquamarine;
            Lua["color.azure"] = Color.Azure;
            Lua["color.beige"] = Color.Beige;
            Lua["color.bisque"] = Color.Bisque;
            Lua["color.black"] = Color.Black;
            Lua["color.blanchedalmond"] = Color.BlanchedAlmond;
            Lua["color.blue"] = Color.Blue;
            Lua["color.aliceblue"] = Color.AliceBlue;
            Lua["color.blueviolet"] = Color.BlueViolet;
            Lua["color.brown"] = Color.Brown;
            Lua["color.burlywood"] = Color.BurlyWood;
            Lua["color.cadetblue"] = Color.CadetBlue;
            Lua["color.chocolate"] = Color.Chocolate;
            Lua["color.chartreuse"] = Color.Chartreuse;
            Lua["color.coral"] = Color.Coral;
            Lua["color.cornsilk"] = Color.Cornsilk;
            Lua["color.crimson"] = Color.Crimson;
            Lua["color.cyan"] = Color.Cyan;
            Lua["color.darkblue"] = Color.DarkBlue;
            Lua["color.darkcyan"] = Color.DarkCyan;
            Lua["color.darkgoldenrod"] = Color.DarkGoldenrod;
            Lua["color.darkgray"] = Color.DarkGray;
            Lua["color.darkgreen"] = Color.DarkGreen;
            Lua["color.darkkhaki"] = Color.DarkKhaki;
            Lua["color.darkmagenta"] = Color.DarkMagenta;
            Lua["color.darkolivegreen"] = Color.DarkOliveGreen;
            Lua["color.darkorange"] = Color.DarkOrange;
            Lua["color.darkorchid"] = Color.DarkOrchid;
            Lua["color.darkred"] = Color.DarkRed;
            Lua["color.darksalmon"] = Color.DarkSalmon;
            Lua["color.darkseagreen"] = Color.DarkSeaGreen;
            Lua["color.darkslateblue"] = Color.DarkSlateBlue;
            Lua["color.darkslategray"] = Color.DarkSlateGray;
            Lua["color.darkturquoise"] = Color.DarkTurquoise;
            Lua["color.darkviolet"] = Color.DarkViolet;
            Lua["color.deeppink"] = Color.DeepPink;
            Lua["color.deepskyblue"] = Color.DeepSkyBlue;
            Lua["color.dimgray"] = Color.DimGray;
            Lua["color.dodgerblue"] = Color.DodgerBlue;
            Lua["color.firebrick"] = Color.Firebrick;
            Lua["color.floralwhite"] = Color.FloralWhite;
            Lua["color.forestgreen"] = Color.ForestGreen;
            Lua["color.fuchsia"] = Color.Fuchsia;
            Lua["color.gainsboro"] = Color.Gainsboro;
            Lua["color.ghostwhite"] = Color.GhostWhite;
            Lua["color.gold"] = Color.Gold;
            Lua["color.goldenrod"] = Color.Goldenrod;
            Lua["color.gray"] = Color.Gray;
            Lua["color.green"] = Color.Green;
            Lua["color.greenyellow"] = Color.GreenYellow;
            Lua["color.honeydew"] = Color.Honeydew;
            Lua["color.hotpink"] = Color.HotPink;
            Lua["color.indigo"] = Color.Indigo;
            Lua["color.indianred"] = Color.IndianRed;
            Lua["color.invory"] = Color.Ivory;
            Lua["color.khaki"] = Color.Khaki;
            Lua["color.lavender"] = Color.Lavender;
            Lua["color.lavenderblush"] = Color.LavenderBlush;
            Lua["color.lawngreen"] = Color.LawnGreen;
            Lua["color.lemonchiffon"] = Color.LemonChiffon;
            Lua["color.lightblue"] = Color.LightBlue;
            Lua["color.lightcoral"] = Color.LightCoral;
            Lua["color.lightcyan"] = Color.LightCyan;
            Lua["color.lightgoldenrodyellow"] = Color.LightGoldenrodYellow;
            Lua["color.lightgray"] = Color.LightGray;
            Lua["color.lightgreen"] = Color.LightGreen;
            Lua["color.lightpink"] = Color.LightPink;
            Lua["color.lightsalmon"] = Color.LightSalmon;
            Lua["color.lavender"] = Color.Lavender;
            Lua["color.lavenderblush"] = Color.LavenderBlush;
            Lua["color.lawngreen"] = Color.LawnGreen;
            Lua["color.lemonchiffon"] = Color.LemonChiffon;
            Lua["color.lime"] = Color.Lime;
            Lua["color.lightseagreen"] = Color.LightSeaGreen;
            Lua["color.lightskyblue"] = Color.LightSkyBlue;
            Lua["color.limegreen"] = Color.LimeGreen;
            Lua["color.linen"] = Color.Linen;
            Lua.RegisterFunction("color.create", this, GetType().GetMethod("CreateColor"));

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
            Lua.DoFile(Path.Combine("scripts", "init.lua"));
            (Lua["Main"] as LuaFunction)?.Call();

            base.Initialize();
        }

        public void EndGame()
        {
            Exit();
        }

        public Color CreateColor()
        {
            return new Color();
        }

        public int Random(int min, int max)
        {
            return random.Next(min, max);
        }

        public Vector2 CreateVector2()
        {
            return new Vector2();
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
            
            Lua.DoFile(Path.Combine("scripts", "loadcontent.lua"));
            (Lua["Main"] as LuaFunction)?.Call();
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

            Lua.DoFile(Path.Combine("scripts", "gamedraw.lua"));
            (Lua["Main"] as LuaFunction)?.Call(gameTime);

            base.Draw(gameTime);
        }
    }
}
