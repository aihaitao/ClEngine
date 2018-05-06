using MonoGame.Extended.Particles;

namespace ClEngine.CoreLibrary.Particle
{
	public class EmitterEventArgs: CoreOperationEventArgs
	{
		public EmitterEventArgs(ParticleEmitter emitter) : base()
		{
			this.Emitter = emitter;
		}

		public ParticleEmitter Emitter { get; private set; }
	}
}