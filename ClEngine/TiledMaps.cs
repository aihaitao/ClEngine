using System.Collections.Generic;
using ClEngine.View.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Graphics.Effects;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;
using MonoGame.Extended.Tiled.Graphics.Effects;
using MonoGame.Extended.ViewportAdapters;

namespace ClEngine
{
	public class TiledMaps : Demo
	{
		public override string Name => "地图";
		private ViewportAdapter _viewportAdapter;
		private Camera2D _camera;
		private TiledMapRenderer _mapRenderer;
		private TiledMap _map;
		private Queue<string> _availableMaps;
		private Effect _customEffect;
		private SpriteBatch _spriteBatch;
		private SpriteFont _smallFont;

		public TiledMaps(MapDraw game) : base(game)
		{
		}

		protected override void Initialize()
		{
			_mapRenderer = new TiledMapRenderer(GraphicsDevice);

			base.Initialize();
		}

		protected override void Update(GameTime gameTime)
		{
			_mapRenderer.Update(_map, gameTime);

			base.Update(gameTime);
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			_availableMaps = new Queue<string>(new[] { "level01" });
			_smallFont = Content.Load<SpriteFont>("msyh9");

			_map = LoadNextMap();
			_camera.LookAt(new Vector2(_map.WidthInPixels, _map.HeightInPixels) * 0.5f);

			var effect = new CustomEffect(GraphicsDevice)
			{
				Alpha = 0.5f,
				TextureEnabled = true,
				VertexColorEnabled = false,
			};

			_customEffect = effect;
		}

		private TiledMap LoadNextMap()
		{
			var name = _availableMaps.Dequeue();
			_map = Content.Load<TiledMap>($"TiledMaps/{name}");
			_availableMaps.Enqueue(name);
			return _map;
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

			DrawText();

			base.Draw(gameTime);
		}

		private void DrawText()
		{
			var textColor = Color.Black;
			_spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);

			var baseTextPosition = new Point2(5, 0);
			var textPosition = baseTextPosition;
			_spriteBatch.DrawString(_smallFont,
				$"地图: {_map.Name};{_map.TileLayers.Count}地图层 @ {_map.Width} x {_map.Height} 瓷砖, {_map.ImageLayers.Count} 图像层",
				textPosition, textColor);

			textPosition = baseTextPosition + new Vector2(0, _smallFont.LineSpacing);
			_spriteBatch.DrawString(_smallFont, $"相机位置 : (x={_camera.Position.X}, y = {_camera.Position.Y})", textPosition,
				textColor);

			_spriteBatch.End();
		}
	}

	public class CustomEffect : DefaultEffect, ITiledMapEffect
	{
		public CustomEffect(GraphicsDevice graphicsDevice)
			: base(graphicsDevice)
		{

		}

		public CustomEffect(GraphicsDevice graphicsDevice, byte[] byteCode)
			: base(graphicsDevice, byteCode)
		{

		}

		public CustomEffect(Effect cloneSource)
			: base(cloneSource)
		{
		}
	}
}