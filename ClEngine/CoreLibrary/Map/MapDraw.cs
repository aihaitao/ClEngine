using System;
using System.IO;
using ClEngine.Tiled;
using ClEngine.Tiled.MapEnum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;

namespace ClEngine.CoreLibrary.Map
{
	public class MapDraw : WpfGame
	{
		private IGraphicsDeviceService _graphicsDeviceManagerWpf;
		private WpfKeyboard _keyboard;
		private WpfMouse _mouse;
		private SpriteBatch _spriteBatch;
		private SpriteFont _smallFont;
		private Camera2D _camera;
		private ViewportAdapter _viewportAdapter;
		private KeyboardState _previousKeyboardState;
		private Tiled.Map _map;
		private Color _color;

		public ViewportAdapter ViewportAdapter { get; private set; }

		public static MapDraw Instance { get; private set; }

		public MapDraw()
		{
			Instance = this;
		}

		protected override void Initialize()
		{
			_graphicsDeviceManagerWpf = new WpfGraphicsDeviceService(this);

			_color = Color.CornflowerBlue;
			_viewportAdapter = new DefaultViewportAdapter(GraphicsDevice);
			_camera = new Camera2D(_viewportAdapter);

			_keyboard = new WpfKeyboard(this);
			_mouse = new WpfMouse(this);
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			
			base.Initialize();
		}

		protected override void LoadContent()
		{
			ViewportAdapter = new DefaultViewportAdapter(GraphicsDevice);
			
			_smallFont = Content.Load<SpriteFont>("msyh9");

			_map = new Tiled.Map();
			_camera.LookAt(new Vector2(_map.Width, _map.Height) * 0.5f);
		}

		private void LookAtMapCenter()
		{
			switch (_map.Orientation)
			{
				case MapOrientation.Orthogonal:
					_camera.LookAt(new Vector2(_map.Width, _map.Height) * 0.5f);
					break;
				case MapOrientation.Isometric:
					_camera.LookAt(new Vector2(0, _map.Width + _map.Height) * 0.5f);
					break;
				case MapOrientation.Staggered:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		protected override void Update(GameTime gameTime)
		{
			var deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
			var mouseState = _mouse.GetState();
			var keyboardState = _keyboard.GetState();

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

			// need to normalize the direction vector incase moving diagonally, but can't normalize the zero vector
			// however, the zero vector means we didn't want to move this frame anyways so all good
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

			if (keyboardState.IsKeyDown(Keys.C))
				LookAtMapCenter();

			_previousKeyboardState = keyboardState;

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(_color);

			GraphicsDevice.BlendState = BlendState.AlphaBlend;
			GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			GraphicsDevice.RasterizerState = RasterizerState.CullNone;

			var viewMatrix = _camera.GetViewMatrix();
			var projectionMatrix =
				Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0f, -1f);

			base.Draw(gameTime);
		}

		public void SetContentRoot()
		{
			Content.RootDirectory = Path.Combine(MainWindow.ProjectPosition, "Content");
		}

		public void SetCurrentMap(string mapName)
		{
			_map = MapHelper.GetMap(mapName);
			LookAtMapCenter();
		}
	}
}