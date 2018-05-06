using MonoGame.Extended.Particles.Modifiers;

namespace CLEngine.PluginInterfaces
{
	public interface IModifierPlugin : IPlugin
	{
		string Category { get; }

		Modifier CreateDefaultInstance();
	}
}