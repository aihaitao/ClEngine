using MonoGame.Extended.Particles;

namespace CLEngine.PluginInterfaces
{
	public interface IEffectSerializationPlugin : IPlugin
	{
		/// <summary>
		/// 粒子效果文件|*.pfx
		/// </summary>
		string FileFilter { get; }

		bool SerializeSuported { get; }
		void Serialize(ParticleEffect effect, string filename);
		bool DeserializeSupported { get; }
		ParticleEffect Deserialize(string filename);
	}
}