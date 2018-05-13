using System.Collections.Generic;
using System.Xml.Serialization;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "layer")]
	public class MapLayer
	{
		private string _name;
		private int _x;
		private int _y;
		private int _width;
		private int _height;
		private float _opacity;
		private bool _visible;
		private string _offsetx;
		private string _offsety;
		private List<MapProperties> _mapProperties;
		private MapData _mapData;

		[XmlAttribute("name")]
		public string Name
		{
			get => _name;
			set => _name = value;
		}

		[XmlAttribute("x")]
		public int X
		{
			get => _x;
			set => _x = value;
		}

		[XmlAttribute("y")]
		public int Y
		{
			get => _y;
			set => _y = value;
		}

		[XmlAttribute("width")]
		public int Width
		{
			get => _width;
			set => _width = value;
		}

		[XmlAttribute("height")]
		public int Height
		{
			get => _height;
			set => _height = value;
		}

		[XmlAttribute("opacity")]
		public float Opacity
		{
			get => _opacity;
			set => _opacity = value;
		}

		[XmlAttribute("visible")]
		public bool Visible
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
		public List<MapProperties> Properties
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