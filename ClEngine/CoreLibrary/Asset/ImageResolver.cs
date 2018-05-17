namespace ClEngine.CoreLibrary.Asset
{
	public class ImageResolver : AssetResolver
	{
		private string _extension => "图片资源 (*.bmp, *.png, *.jpg)|*.bmp;*.png;*.jpg";
		private string _watcherExtension => "*.bmp|*.png|*.jpg";

		public override string Extension => _extension;
		public override string WatcherExtension => _watcherExtension;

		public ImageResolver() : base("Image")
		{
		}
	}
}