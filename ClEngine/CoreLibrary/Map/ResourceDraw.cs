using ClEngine.CoreLibrary.Asset;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
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
		private ViewportAdapter _viewportAdapter;
		private KeyboardState _previousKeyboardState;

		protected override void Initialize()
		{
			_graphicsDeviceManagerWpf = new WpfGraphicsDeviceService(this);

			_keyboard = new WpfKeyboard(this);
			_mouse = new WpfMouse(this);
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			_viewportAdapter = new DefaultViewportAdapter(GraphicsDevice);
			_camera = new Camera2D(_viewportAdapter);

			base.Initialize();
		}

		protected override void LoadContent()
		{
			var viewportAdapter = new DefaultViewportAdapter(GraphicsDevice);
			_camera = new Camera2D(viewportAdapter);
			_smallFont = Content.Load<SpriteFont>("msyh9");

			if (_texture2D != null)
				_camera.LookAt(new Vector2(_texture2D.Width, _texture2D.Height) * 0.5f);

			base.LoadContent();
		}

		protected override void UnloadContent()
		{
			_texture2D?.Dispose();
			_spriteBatch?.Dispose();

		    base.UnloadContent();
        }

		protected override void Update(GameTime gameTime)
		{
			var deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
			var mouseState = _mouse.GetState();
			var keyboardState = _keyboard.GetState();
			var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
			var p = _camera.ScreenToWorld(mouseState.X, mouseState.Y);

			const float cameraSpeed = 500f;
			const float zoomSpeed = 0.3f;

			var moveDirection = Vector2.Zero;

			if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
				moveDirection -= Vector2.UnitY;

			if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
				moveDirection -= Vector2.UnitX;

			if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
				moveDirection += Vector2.UnitY;

			if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
				moveDirection += Vector2.UnitX;

			var isCameraMoving = moveDirection != Vector2.Zero;
			if (isCameraMoving)
			{
				moveDirection.Normalize();
				_camera.Move(moveDirection * cameraSpeed * deltaSeconds);
			}

			if (keyboardState.IsKeyDown(Keys.R))
				_camera.ZoomIn(zoomSpeed * deltaSeconds);

			if (keyboardState.IsKeyDown(Keys.F))
				_camera.ZoomOut(zoomSpeed * deltaSeconds);

			if (keyboardState.IsKeyDown(Keys.Z))
				_camera.Position = Vector2.Zero;

			if (keyboardState.IsKeyDown(Keys.X))
				_camera.LookAt(Vector2.Zero);

			_previousKeyboardState = keyboardState;

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			var viewMatrix = _camera.GetViewMatrix();
			_spriteBatch.Begin(blendState: BlendState.AlphaBlend, transformMatrix: viewMatrix);

			if (_texture2D != null)
			{
				_spriteBatch.Draw(_texture2D, Vector2.One, Color.White);
			}
			
			_spriteBatch.End();

			base.Draw(gameTime);
		}

		public void LoadResource(string name, object type)
		{
			_texture2D = null;

			if (type is ImageResolver)
			{
				_texture2D = Content.Load<Texture2D>(name);
			}
		}
	}
}