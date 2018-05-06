namespace ClEngine.CoreLibrary.Particle
{
	public class NewTextureReferenceEventArgs : CoreOperationEventArgs
	{
		public NewTextureReferenceEventArgs(string filePath)
			: base()
		{
			this.FilePath = filePath;
		}

		public string FilePath { get; private set; }
		public TextureReference AddedTextureReference { get; set; }
	}
}