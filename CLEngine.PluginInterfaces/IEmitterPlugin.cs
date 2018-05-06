using MonoGame.Extended.Particles;

namespace CLEngine.PluginInterfaces
{
	public interface IEmitterPlugin : IPlugin
	{
		ParticleEmitter CreateDefaultInstance();
	}
}