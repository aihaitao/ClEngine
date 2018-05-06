using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Particles;

namespace ClEngine.CoreLibrary.Particle
{
	public sealed class SpriteBatchRenderer : Renderer
	{
		private SpriteBatch Batch;
		private BlendState NonPremultipliedAdditive { get; set; }
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Batch?.Dispose();
			}

			base.Dispose(disposing);
		}

		public override void LoadContent(ContentManager content)
		{
			Guard.IsTrue(GraphicsDeviceService == null, "GraphicsDeviceService属性尚未使用有效值初始化.");

			if (Batch == null)
			{
				var graphicsDeviceService = GraphicsDeviceService;
				if (graphicsDeviceService != null)
					Batch = new SpriteBatch(graphicsDeviceService.GraphicsDevice);
			}

			if (NonPremultipliedAdditive == null)
				NonPremultipliedAdditive = new BlendState
				{
					AlphaBlendFunction = BlendFunction.Add,
					AlphaDestinationBlend = Blend.One,
					AlphaSourceBlend = Blend.SourceAlpha,
					ColorBlendFunction = BlendFunction.Add,
					ColorDestinationBlend = Blend.One,
					ColorSourceBlend = Blend.SourceAlpha,
				};
		}
		public override void RenderEmitter(ParticleEmitter emitter, ref Matrix transform)
		{
			Guard.ArgumentNull("emitter", emitter);
			Guard.IsTrue(this.Batch == null, "SpriteBatchRenderer还没有准备好！ 还未加载资源?");

			if (emitter.TextureRegion.Texture != null && emitter.ActiveParticles > 0)
			{
				// Calculate the source rectangle and origin offset of the Particle texture...
				Rectangle source = new Rectangle(0, 0, emitter.TextureRegion.Texture.Width, emitter.TextureRegion.Texture.Height);
				Vector2 origin = new Vector2(source.Width / 2f, source.Height / 2f);

				for (int i = 0; i < emitter.ActiveParticles; i++)
				{
					// TODO: drawSpriteBatch
				}

				this.Batch.End();
			}
		}

		private BlendState GetBlendState(BlendState emitterBlendMode)
		{
			if (emitterBlendMode == BlendState.AlphaBlend)
			{
				return BlendState.NonPremultiplied;
			}
			else if (emitterBlendMode == BlendState.Additive)
			{
				return NonPremultipliedAdditive;
			}
			else
			{
				throw new InvalidEnumArgumentException("emitterBlendMode", int.Parse(emitterBlendMode.ToString()), typeof(BlendState));
			}
		}
	}
}