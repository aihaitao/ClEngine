using MonoGame.Extended.Particles.Modifiers;

namespace ClEngine.CoreLibrary.Particle
{
	public class CloneModifierEventArgs : CoreOperationEventArgs
	{
		public CloneModifierEventArgs(Modifier prototype)
			: base()
		{
			this.Prototype = prototype;
		}

		public Modifier Prototype { get; private set; }
		public Modifier AddedModifier { get; set; }
	}
}