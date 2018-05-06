using MonoGame.Extended.Particles;

namespace ClEngine.CoreLibrary.Particle
{
	public class EmitterReinitialisedEventArgs : EmitterEventArgs
	{
		public EmitterReinitialisedEventArgs(ParticleEmitter emitter, int budget, float term)
			: base(emitter)
		{
			this.Budget = budget;
			this.Term = term;
		}

		public int Budget { get; private set; }
		public float Term { get; private set; }
	}
}