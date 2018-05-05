using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;

namespace ClEngine.CoreLibrary.Map
{
	public class ResourceDraw : WpfGame
	{
		private IGraphicsDeviceService _graphicsDeviceManagerWpf;
		private WpfKeyboard _keyboard;
		private WpfMouse _mouse;
		private SpriteBatch _spriteBatch;
		private SpriteFont _smallFont;
		private Texture2D _texture2D;
		private Camera2D _camera;
		private ParticleEffect _particleEffect;

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
			var viewportAdapter = new DefaultViewportAdapter(GraphicsDevice);
			_camera = new Camera2D(viewportAdapter);
			_smallFont = Content.Load<SpriteFont>("msyh9");

			base.LoadContent();
		}

		protected override void Update(GameTime gameTime)
		{
			var mouseState = _mouse.GetState();
			var keyboardState = _keyboard.GetState();
			var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
			var p = _camera.ScreenToWorld(mouseState.X, mouseState.Y);
			
			_particleEffect?.Update(deltaTime);

			if (mouseState.LeftButton == ButtonState.Released)
				_particleEffect?.Trigger(new Vector2(p.X, p.Y));

			_particleEffect?.Trigger(new Vector2(GraphicsDevice.Viewport.Width / 2,GraphicsDevice.Viewport.Height / 2));

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);
			
			_spriteBatch.Begin(blendState: BlendState.AlphaBlend, transformMatrix: _camera.GetViewMatrix());
			if (_particleEffect != null)
			{
				_spriteBatch.Draw(_particleEffect);
			}

			if (_texture2D != null)
			{
				_spriteBatch.Draw(_texture2D, Vector2.One,
					new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
			}
			
			_spriteBatch.End();

			base.Draw(gameTime);
		}

		public void LoadResource(string name, ResourceType type)
		{
			_texture2D = null;
			_particleEffect = null;

			switch (type)
			{
				case ResourceType.Image:
					_texture2D = Content.Load<Texture2D>(name);
					break;
				case ResourceType.Animation:
					break;
				case ResourceType.Sound:
					break;
				case ResourceType.Particle:
					break;
				case ResourceType.Font:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}
	}
}