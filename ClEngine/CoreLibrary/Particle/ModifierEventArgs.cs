using MonoGame.Extended.Particles.Modifiers;

namespace ClEngine.CoreLibrary.Particle
{
	public class ModifierEventArgs : CoreOperationEventArgs
	{
		public ModifierEventArgs(Modifier modifier)
			: base()
		{
			this.Modifier = modifier;
		}
		public Modifier Modifier { get; private set; }
	}
}