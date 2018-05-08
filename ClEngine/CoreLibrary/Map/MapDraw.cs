using System;
using System.IO;
using System.Xml.Serialization;
using ClEngine.CoreLibrary.Map.Effect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;
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
		private Microsoft.Xna.Framework.Graphics.Effect _customEffect;
		private Camera2D _camera;
		private ViewportAdapter _viewportAdapter;
		private TiledMapRenderer _mapRenderer;
		private KeyboardState _previousKeyboardState;
		private TiledMap _map;

		public ViewportAdapter ViewportAdapter { get; private set; }

		public static MapDraw Instance { get; private set; }

		public MapDraw()
		{
			Instance = this;
			
			using (var stream = new FileStream("F://Test//Content//Map//untitled.tmx", FileMode.Open, FileAccess.Read))
			{
				var serializer = new XmlSerializer(typeof(Map));
				var result = serializer.Deserialize(stream);
			}
		}

		protected override void Initialize()
		{
			_graphicsDeviceManagerWpf = new WpfGraphicsDeviceService(this);

			_viewportAdapter = new DefaultViewportAdapter(GraphicsDevice);
			_camera = new Camera2D(_viewportAdapter);
			_mapRenderer = new TiledMapRenderer(GraphicsDevice);

			_keyboard = new WpfKeyboard(this);
			_mouse = new WpfMouse(this);
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			
			base.Initialize();
		}

		protected override void LoadContent()
		{
			ViewportAdapter = new DefaultViewportAdapter(GraphicsDevice);
			
			_smallFont = Content.Load<SpriteFont>("msyh9");

			_map = new TiledMap("默认地图", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 64, 32,
				TiledMapTileDrawOrder.LeftUp, TiledMapOrientation.Orthogonal);
			_camera.LookAt(new Vector2(_map.WidthInPixels, _map.HeightInPixels) * 0.5f);

			var effect = new CustomEffect(GraphicsDevice)
			{
				Alpha = 0.5f,
				TextureEnabled = true,
				VertexColorEnabled = false,
			};

			_customEffect = effect;
		}

		private void LookAtMapCenter()
		{
			switch (_map.Orientation)
			{
				case TiledMapOrientation.Orthogonal:
					_camera.LookAt(new Vector2(_map.WidthInPixels, _map.HeightInPixels) * 0.5f);
					break;
				case TiledMapOrientation.Isometric:
					_camera.LookAt(new Vector2(0, _map.HeightInPixels + _map.TileHeight) * 0.5f);
					break;
				case TiledMapOrientation.Staggered:
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

			_mapRenderer.Update(_map, gameTime);

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
			GraphicsDevice.Clear(Color.CornflowerBlue);

			GraphicsDevice.BlendState = BlendState.AlphaBlend;
			GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			GraphicsDevice.RasterizerState = RasterizerState.CullNone;

			var viewMatrix = _camera.GetViewMatrix();
			var projectionMatrix =
				Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0f, -1f);

			_mapRenderer.Draw(_map, ref viewMatrix, ref projectionMatrix, _customEffect);

			base.Draw(gameTime);
		}

		public void SetContentRoot()
		{
			Content.RootDirectory = Path.Combine(MainWindow.ProjectPosition, "Content");
			//_map = Content.Load<TiledMap>(@"Map/untitled");
			LookAtMapCenter();
		}

		public void SetCurrentMap(string mapName)
		{
			_map = Content.Load<TiledMap>(mapName);
		}
	}
}