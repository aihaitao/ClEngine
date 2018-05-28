namespace ClEngine.CoreLibrary.Asset
{
	public class ImageResolver : AssetResolver
	{
		private string _extension => "图片资源 (*.bmp, *.png, *.jpg)|*.bmp;*.png;*.jpg";

		public override string Extension => _extension;

		public ImageResolver() : base("Image")
		{
		}
	}
}