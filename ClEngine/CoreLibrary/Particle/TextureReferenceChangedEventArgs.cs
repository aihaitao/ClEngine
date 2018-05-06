using MonoGame.Extended.Particles;

namespace ClEngine.CoreLibrary.Particle
{
	public class TextureReferenceChangedEventArgs : EmitterEventArgs
	{
		public TextureReferenceChangedEventArgs(ParticleEmitter emitter, TextureReference textureReference)
			: base(emitter)
		{
			this.TextureReference = textureReference;
		}

		public TextureReference TextureReference { get; private set; }
	}
}