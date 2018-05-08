namespace ClEngine.Tiled
{
	/// <remarks/>
	[System.Serializable]
	public class MapTilesetTile
	{

		private MapTilesetTileProperties _propertiesField;

		private MapTilesetTileImage _imageField;

		private string _idField;

		private string _gidField;

		/// <remarks/>
		public MapTilesetTileProperties Properties
		{
			get => _propertiesField;
			set => _propertiesField = value;
		}

		/// <remarks/>
		public MapTilesetTileImage Image
		{
			get => _imageField;
			set => _imageField = value;
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
		public string Gid
		{
			get => _gidField;
			set => _gidField = value;
		}
	}
}