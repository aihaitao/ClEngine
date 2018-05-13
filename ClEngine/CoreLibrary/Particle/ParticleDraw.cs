using System;
using System.Collections.Generic;
using ClEngine.Particle;
using ClEngine.Particle.Profiles;
using ClEngine.View.Particle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;

namespace ClEngine.CoreLibrary.Particle
{
	public class ParticleDraw : WpfGame
	{
		private IGraphicsDeviceService _graphicsDeviceService;
		private WpfKeyboard _keyboard;
		private WpfMouse _mouse;
		private Camera2D _camera;
		private Texture2D _particleTexture;
		private SpriteBatch _spriteBatch;
		private SpriteFont _smallFont;
		private readonly FramesPerSecondCounter _fpsCounter = new FramesPerSecondCounter();

		public ParticleEffect ParticleEffect { get; set; }
		protected override void Initialize()
		{
			_graphicsDeviceService = new WpfGraphicsDeviceService(this);
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			_keyboard = new WpfKeyboard(this);
			_mouse = new WpfMouse(this);

			base.Initialize();
		}

		protected override void LoadContent()
		{
			var viewportAdapter = new DefaultViewportAdapter(GraphicsDevice);
			_camera = new Camera2D(viewportAdapter);

			_smallFont = Content.Load<SpriteFont>("msyh9");

			_particleTexture = new Texture2D(GraphicsDevice, 1, 1);
			_particleTexture.SetData(new[] {Color.White});

			ParticleInit(new TextureRegion2D(_particleTexture));

			base.LoadContent();
		}

		protected override void UnloadContent()
		{
			_particleTexture?.Dispose();
		}

		protected override void Update(GameTime gameTime)
		{
			var mouseState = _mouse.GetState();
			var keyboardState = _keyboard.GetState();
			var deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
			var p = _camera.ScreenToWorld(mouseState.X, mouseState.Y);

			ParticleEffect.Update(deltaTime);
			_fpsCounter.Update(gameTime);

			if (mouseState.LeftButton == ButtonState.Pressed)
			{
				ParticleEffect.Position = new Vector2(p.X, p.Y);
				ParticleEditor.Instance.UpdateData();
			}


			if (mouseState.RightButton == ButtonState.Pressed)
			{
				ParticleEffect.Trigger(new Vector2(p.X, p.Y));
				ParticleEditor.Instance.UpdateData();
			}


			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			_fpsCounter.Draw(gameTime);

			_spriteBatch.Begin(blendState: BlendState.AlphaBlend, transformMatrix: _camera.GetViewMatrix());
			_spriteBatch.Draw(ParticleEffect);
			_spriteBatch.DrawString(_smallFont, $"当前fps： {_fpsCounter.FramesPerSecond}", Vector2.Zero, Color.White);
			_spriteBatch.End();

			base.Draw(gameTime);
		}

		private void ParticleInit(TextureRegion2D textureRegion)
		{
			ParticleEffect = new ParticleEffect
			{
				Emitters = new List<ParticleEmitter>
				{
					new ParticleEmitter("基础发射器", textureRegion,5000,TimeSpan.FromSeconds(1),Profile.Point())
					{
						Parameters =
						{
							Speed = new Range<float>(25f),
							Quantity = 10,
							Opacity = 1f,
							Color = Color.White.ToHsl(),
						},
						AutoTrigger = true,
					},
				}
			};

			ParticleEditor.Instance.SelectObject(ParticleEffect);
		}
	}
}