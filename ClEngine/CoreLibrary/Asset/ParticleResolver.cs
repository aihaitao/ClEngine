namespace ClEngine.CoreLibrary.Asset
{
	public class ParticleResolver : AssetResolver
	{
		private string _extension => "粒子资源 (*.em, *.xml)|*.em;*.xml";
		private string _watcherExtension => "*.em|*.xml";
		public ParticleResolver() : base("Particle")
		{
		}

		public override string Extension => _extension;
		public override string WatcherExtension => _watcherExtension;
	}
}