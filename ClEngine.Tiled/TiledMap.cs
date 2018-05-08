
// 注意: 生成的代码可能至少需要 .NET Framework 4.5 或 .NET Core/Standard 2.0。

namespace ClEngine.Tiled
{
	/// <remarks/>
	[System.Serializable]
	public class Map
	{

		private MapProperties _propertiesField;

		private MapTileset _tilesetField;

		private MapLayer _layerField;

		private string _widthField;

		private string _heightField;

		private string _versionField;

		private string _tilewidthField;

		private string _tileheightField;

		private string _orientationField;

		private string _schemaLocationField;

		/// <remarks/>
		public MapProperties Properties
		{
			get => _propertiesField;
			set => _propertiesField = value;
		}

		/// <remarks/>
		public MapTileset Tileset
		{
			get => _tilesetField;
			set => _tilesetField = value;
		}

		/// <remarks/>
		public MapLayer Layer
		{
			get => _layerField;
			set => _layerField = value;
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
		public string Version
		{
			get => _versionField;
			set => _versionField = value;
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
		public string Orientation
		{
			get => _orientationField;
			set => _orientationField = value;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
		public string SchemaLocation
		{
			get => _schemaLocationField;
			set => _schemaLocationField = value;
		}
	}
}