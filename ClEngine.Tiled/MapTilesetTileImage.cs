namespace ClEngine.Tiled
{
	/// <remarks/>
	[System.Serializable]
	public class MapTilesetTileImage
	{

		private MapTilesetTileImageData _dataField;

		private string _widthField;

		private string _heightField;

		private string _idField;

		private string _transField;

		private string _formatField;

		private string _sourceField;

		/// <remarks/>
		public MapTilesetTileImageData Data
		{
			get => _dataField;
			set => _dataField = value;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute]
		public string Width
		{
			get => _widthField;
			set => _widthField = value;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute]
		public string Height
		{
			get => _heightField;
			set => _heightField = value;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute]
		public string Id
		{
			get => _idField;
			set => _idField = value;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute]
		public string Trans
		{
			get => _transField;
			set => _transField = value;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute]
		public string Format
		{
			get => _formatField;
			set => _formatField = value;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute]
		public string Source
		{
			get => _sourceField;
			set => _sourceField = value;
		}
	}
}