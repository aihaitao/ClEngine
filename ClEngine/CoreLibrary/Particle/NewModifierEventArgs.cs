using CLEngine.PluginInterfaces;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;

namespace ClEngine.CoreLibrary.Particle
{
	public class NewModifierEventArgs : CoreOperationEventArgs
	{
		public NewModifierEventArgs(ParticleEmitter parentEmitter, IModifierPlugin plugin)
			: base()
		{
			this.ParentEmitter = parentEmitter;
			this.Plugin = plugin;
		}

		public ParticleEmitter ParentEmitter { get; private set; }

		public IModifierPlugin Plugin { get; private set; }
		public Modifier AddedModifier { get; set; }
	}
}