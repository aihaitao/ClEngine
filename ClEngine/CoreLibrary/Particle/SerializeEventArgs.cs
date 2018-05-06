using CLEngine.PluginInterfaces;

namespace ClEngine.CoreLibrary.Particle
{
	public class SerializeEventArgs : CoreOperationEventArgs
	{
		public SerializeEventArgs(IEffectSerializationPlugin plugin, string filePath)
			: base()
		{
			Plugin = plugin;
			FilePath = filePath;
		}

		public IEffectSerializationPlugin Plugin { get; private set; }

		public string FilePath { get; private set; }
	}
}