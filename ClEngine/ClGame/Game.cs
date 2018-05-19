using System;
using System.IO;
using ClEngine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;
using NLua;

namespace ClGame
{
    public class Game : WpfGame
    {
        private IGraphicsDeviceService _graphicsDeviceService;
        private WpfKeyboard _keyboard;
        private WpfMouse _mouse;
        private Lua _lua;

        protected override void Initialize()
        {
            _graphicsDeviceService = new WpfGraphicsDeviceService(this);
            _keyboard = new WpfKeyboard(this);
            _mouse = new WpfMouse(this);

            _lua = new Lua
            {
                ["print"] = new Action<object>(Print),
            };

            _lua.NewTable("system");
            _lua["system.width"] = _graphicsDeviceService.GraphicsDevice.Viewport.Width;
            _lua["system.height"] = _graphicsDeviceService.GraphicsDevice.Viewport.Height;

            _lua.DoFile(Path.Combine("scripts", "init.lua"));
            (_lua["Main"] as LuaFunction)?.Call();

            base.Initialize();
        }

        private void Print(object message)
        {
            MessageCache.Messages.Add(new MessageModel {Message = message});
            Console.WriteLine(MessageCache.Messages.Count);
        }

        protected override void Update(GameTime time)
        {
            var mouseState = _mouse.GetState();
            var keyboardState = _keyboard.GetState();

            _lua.DoFile(Path.Combine("scripts", "input.lua"));
            (_lua["Main"] as LuaFunction)?.Call(mouseState, keyboardState);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
        }
    }
}