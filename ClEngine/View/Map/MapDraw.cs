using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;

namespace ClEngine.View.Map
{
	public class MapDraw : WpfGame
	{
		private IGraphicsDeviceService _graphicsDeviceManagerWpf;
		private WpfKeyboard _keyboard;
		private WpfMouse _mouse;
		private SpriteFont _defaultFont;
		private SpriteBatch _spriteBatch;
		private SpriteFont _smallFont;
		private readonly Dictionary<string, Demo> _demos;
		private Demo _currentDemo;

		public ViewportAdapter ViewportAdapter { get; private set; }

		public static MapDraw Instance { get; private set; }

		public MapDraw()
		{
			Instance = this;

			_demos = new Demo[]
			{
				new TiledMaps(this), 
			}.ToDictionary(d => d.Name);
		}

		protected override void Initialize()
		{
			_graphicsDeviceManagerWpf = new WpfGraphicsDeviceService(this);

			_keyboard = new WpfKeyboard(this);
			_mouse = new WpfMouse(this);
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			
			base.Initialize();
		}

		protected override void LoadContent()
		{
			ViewportAdapter = new DefaultViewportAdapter(GraphicsDevice);

			_defaultFont = Content.Load<SpriteFont>("defaultFont");
			_smallFont = Content.Load<SpriteFont>("msyh9");

			base.LoadContent();
		}

		private void Exit()
		{
			
		}

		private void LoadDemo(string name)
		{
			_currentDemo?.Unload();
			_currentDemo = _demos[name];
			_currentDemo.Load();
		}

		protected override void Update(GameTime gameTime)
		{
			var mouseState = _mouse.GetState();
			var keyboardState = _keyboard.GetState();
			
			_currentDemo?.OnUpdate(gameTime);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			base.Draw(gameTime);

			_currentDemo?.OnDraw(gameTime);
		}

		public void SetContentRoot()
		{
			Content.RootDirectory = Path.Combine(MainWindow.ProjectPosition, "Content");
		}
	}
}