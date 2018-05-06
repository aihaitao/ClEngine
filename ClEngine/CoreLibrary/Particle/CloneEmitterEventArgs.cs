using MonoGame.Extended.Particles;

namespace ClEngine.CoreLibrary.Particle
{
	public class CloneEmitterEventArgs : CoreOperationEventArgs
	{
		public CloneEmitterEventArgs(ParticleEmitter prototype)
			: base()
		{
			this.Prototype = prototype;
		}

		public ParticleEmitter Prototype { get; private set; }

		public ParticleEmitter AddedEmitter { get; set; }
	}
}