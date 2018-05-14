using System.IO;

namespace ClEngine.CoreLibrary.Asset
{
	public class ImageResolver : AssetResolver
	{
		public ImageResolver() : base("Image")
		{
		}

		protected override void MoveAsset(ref string orginPath)
		{
			var fileInfo = new FileInfo(orginPath);
			var destPosition = Path.Combine(StoragePath, fileInfo.Name);
			if (!Directory.Exists(StoragePath))
				Directory.CreateDirectory(StoragePath);

			if (!orginPath.Equals(destPosition) && !File.Exists(destPosition))
				File.Copy(orginPath, destPosition);

			orginPath = destPosition;
		}

		protected override void CompileAsset()
		{
		}
	}
}