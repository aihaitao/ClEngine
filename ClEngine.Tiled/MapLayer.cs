namespace ClEngine.Tiled
{
	/// <remarks/>
	[System.Serializable]
	public class MapLayer
	{

		private MapLayerProperties _propertiesField;

		private MapLayerData _dataField;

		private string _widthField;

		private string _heightField;

		private string _xField;

		private string _yField;

		private string _nameField;

		private string _opacityField;

		private string _visibleField;

		/// <remarks/>
		public MapLayerProperties Properties
		{
			get => _propertiesField;
			set => _propertiesField = value;
		}

		/// <remarks/>
		public MapLayerData Data
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
		public string X
		{
			get => _xField;
			set => _xField = value;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute]
		public string Y
		{
			get => _yField;
			set => _yField = value;
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
		public string Opacity
		{
			get => _opacityField;
			set => _opacityField = value;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute]
		public string Visible
		{
			get => _visibleField;
			set => _visibleField = value;
		}
	}
}