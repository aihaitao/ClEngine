namespace ClEngine.CoreLibrary.Asset
{
	public class FontResolver : AssetResolver
	{
		private string _extension => "字体资源 (*.spritefont, *.fnt, *.png)|*.spritefont;*.fnt;*.png";
		private string _watcherExtension => "*.spritefont|*.fnt";

		public override string Extension => _extension;
		public override string WatcherExtension => _watcherExtension;

		public FontResolver() : base("Font")
		{
		}
	}
}