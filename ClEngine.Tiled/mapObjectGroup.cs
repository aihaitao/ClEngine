using System.Collections.Generic;
using System.Xml.Serialization;
using ClEngine.Tiled.MapEnum;

namespace ClEngine.Tiled
{

	[System.Serializable, XmlRoot(ElementName = "objectgroup")]
	public class MapObjectGroup
	{
		private string _name;
		private string _color;
		private string _x;
		private string _y;
		private string _width;
		private string _height;
		private string _opacity;
		private string _visible;
		private string _offsetx;
		private string _offsety;
		private MapDrawOrder _draworder;
		private MapProperties _properties;
		private List<MapObject> _object;

		[XmlAttribute("name")]
		public string Name
		{
			get => _name;
			set => _name = value;
		}

		[XmlAttribute("color")]
		public string Color
		{
			get => _color;
			set => _color = value;
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
		public string offsetx
		{
			get => _offsetx;
			set => _offsetx = value;
		}

		[XmlAttribute("offsety")]
		public string offsety
		{
			get => _offsety;
			set => _offsety = value;
		}

		[XmlAttribute("draworder")]
		public MapDrawOrder Draworder
		{
			get => _draworder;
			set => _draworder = value;
		}

		[XmlElement(ElementName = "properties")]
		public MapProperties Properties
		{
			get => _properties;
			set => _properties = value;
		}

		[XmlElement(ElementName = "object")]
		public List<MapObject> Object
		{
			get => _object;
			set => _object = value;
		}
	}
}