namespace ClEngine.Tiled
{
	/// <remarks/>
	[System.Serializable]
	public class MapTileset
	{

		private MapTilesetImage _imageField;

		private MapTilesetTile _tileField;

		private string _tilewidthField;

		private string _tileheightField;

		private string _sourceField;

		private string _nameField;

		private string _marginField;

		private string _spacingField;

		private string _firstgidField;

		/// <remarks/>
		public MapTilesetImage Image
		{
			get => _imageField;
			set => _imageField = value;
		}

		/// <remarks/>
		public MapTilesetTile Tile
		{
			get => _tileField;
			set => _tileField = value;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute]
		public string Tilewidth
		{
			get => _tilewidthField;
			set => _tilewidthField = value;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute]
		public string Tileheight
		{
			get => _tileheightField;
			set => _tileheightField = value;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute]
		public string Source
		{
			get => _sourceField;
			set => _sourceField = value;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute]
		public string Name
		{
			get => _nameField;
			set => _nameField = value;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute]
		public string Margin
		{
			get => _marginField;
			set => _marginField = value;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute]
		public string Spacing
		{
			get => _spacingField;
			set => _spacingField = value;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute]
		public string Firstgid
		{
			get => _firstgidField;
			set => _firstgidField = value;
		}
	}
}