using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Particles;

namespace ClEngine.CoreLibrary.Particle
{
	public abstract class Renderer : IDisposable
	{
		public IGraphicsDeviceService GraphicsDeviceService;

		protected virtual void Dispose(bool disposing) { }

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		~Renderer()
		{
			this.Dispose(false);
		}

		public virtual void LoadContent(ContentManager content) { }

		public virtual void RenderEmitter(ParticleEmitter emitter)
		{
			Guard.ArgumentNull("emitter", emitter);

			Matrix ident = Matrix.Identity;

			this.RenderEmitter(emitter, ref ident);
		}
		public abstract void RenderEmitter(ParticleEmitter emitter, ref Matrix transform);

		public virtual void RenderEffect(ParticleEffect effect)
		{
			Guard.ArgumentNull("effect", effect);

			Matrix ident = Matrix.Identity;

			this.RenderEffect(effect, ref ident);
		}

		public virtual void RenderEffect(ParticleEffect effect, ref Matrix transform)
		{
			Guard.ArgumentNull("effect", effect);

			for (int i = 0; i < effect.Emitters.Count; i++)
				this.RenderEmitter(effect.Emitters[i], ref transform);
		}
	}
}