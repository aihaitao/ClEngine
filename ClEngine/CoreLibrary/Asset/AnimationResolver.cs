namespace ClEngine.CoreLibrary.Asset
{
	public sealed class AnimationResolver : AssetResolver
	{
		private string _watcherExtension => "*.ani";
		private string _extension => "动画资源 (*.ani)|*.ani";

		public AnimationResolver() : base("Animation")
		{
		}

		public override string Extension => _extension;
		public override string WatcherExtension => _watcherExtension;
	}
}