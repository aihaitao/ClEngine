namespace ClEngine.CoreLibrary.Asset
{
	public class FontResolver : AssetResolver
	{
		private string _extension => "字体资源 (*.spritefont, *.fnt, *.png)|*.spritefont;*.fnt;*.png";

		public override string Extension => _extension;

		public FontResolver() : base("Font")
		{
		}
	}
}