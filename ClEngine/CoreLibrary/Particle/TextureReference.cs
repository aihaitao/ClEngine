using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace ClEngine.CoreLibrary.Particle
{
	public class TextureReference
	{
		public TextureReference(string filePath)
		{
			this.FilePath = filePath;

			using (var inputStream = File.OpenRead(filePath))
			{
				this.Texture = Texture2D.FromStream(GraphicsDeviceService.Instance.GraphicsDevice, inputStream);
			}
		}

		public string FilePath { get; private set; }
		public Texture2D Texture { get; private set; }

		public string GetAssetName()
		{
			if (string.IsNullOrEmpty(this.FilePath))
				throw new InvalidOperationException();

			return Path.GetFileNameWithoutExtension(this.FilePath);
		}
	}
}