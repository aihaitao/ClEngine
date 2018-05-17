namespace ClEngine.CoreLibrary.Asset
{
	public class SoundResolver : AssetResolver
	{
		private string _watcherExtension => "*.wav|*.mp3";
		private string _extension => "声音资源 (*.wav, *.mp3)|*.wav;*.mp3";

		public SoundResolver() : base("Sound")
		{
		}

		public override string Extension => _extension;
		public override string WatcherExtension => _watcherExtension;
	}
}