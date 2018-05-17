namespace ClEngine.CoreLibrary.Asset
{
	public class MapResolver : AssetResolver
	{
		public MapResolver() : base("Map")
		{
			UseBundle = false;
		}

		public override string Extension { get; }
		public override string WatcherExtension { get; }
	}
}