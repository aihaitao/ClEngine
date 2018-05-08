namespace ClEngine.Tiled
{
	/// <remarks/>
	[System.Serializable]
	public class MapTilesetImageData
	{

		private string _encodingField;

		private string _compressionField;

		private string _valueField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute]
		public string Encoding
		{
			get => _encodingField;
			set => _encodingField = value;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute]
		public string Compression
		{
			get => _compressionField;
			set => _compressionField = value;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlTextAttribute]
		public string Value
		{
			get => _valueField;
			set => _valueField = value;
		}
	}
}