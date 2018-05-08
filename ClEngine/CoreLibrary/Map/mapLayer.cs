using System.Xml.Serialization;

namespace ClEngine.CoreLibrary.Map
{
	[System.Serializable, XmlRoot(ElementName = "layer")]
	public class MapLayer
	{
		private string _name;
		private string _x;
		private string _y;
		private string _width;
		private string _height;
		private string _opacity;
		private string _visible;
		private string _offsetx;
		private string _offsety;
		private MapProperties _mapProperties;
		private MapData _mapData;

		[XmlAttribute("name")]
		public string Name
		{
			get => _name;
			set => _name = value;
		}

		[XmlAttribute("x")]
		public string X
		{
			get => _x;
			set => _x = value;
		}

		[XmlAttribute("y")]
		public string Y
		{
			get => _y;
			set => _y = value;
		}

		[XmlAttribute("width")]
		public string Width
		{
			get => _width;
			set => _width = value;
		}

		[XmlAttribute("height")]
		public string Height
		{
			get => _height;
			set => _height = value;
		}

		[XmlAttribute("opacity")]
		public string Opacity
		{
			get => _opacity;
			set => _opacity = value;
		}

		[XmlAttribute("visible")]
		public string Visible
		{
			get => _visible;
			set => _visible = value;
		}

		[XmlAttribute("offsetx")]
		public string Offsetx
		{
			get => _offsetx;
			set => _offsetx = value;
		}

		[XmlAttribute("offsety")]
		public string Offsety
		{
			get => _offsety;
			set => _offsety = value;
		}

		[XmlElement(ElementName = "properties")]
		public MapProperties Properties
		{
			get => _mapProperties;
			set => _mapProperties = value;
		}

		[XmlElement(ElementName = "data")]
		public MapData Data
		{
			get => _mapData;
			set => _mapData = value;
		}
	}
}