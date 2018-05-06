using CLEngine.PluginInterfaces;
using MonoGame.Extended.Particles;

namespace ClEngine.CoreLibrary.Particle
{
	public class NewEmitterEventArgs : CoreOperationEventArgs
	{
		public NewEmitterEventArgs(IEmitterPlugin plugin, int budget, float term)
			: base()
		{
			Plugin = plugin;
			Budget = budget;
			Term = term;
		}

		public IEmitterPlugin Plugin { get; private set; }

		public int Budget { get; private set; }

		public float Term { get; private set; }

		public ParticleEmitter AddedEmitter { get; set; }
	}
}