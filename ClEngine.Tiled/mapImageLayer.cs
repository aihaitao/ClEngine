using System.Collections.Generic;
using System.Xml.Serialization;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "imagelayer")]
	public class MapImageLayer
	{
		private string _name;
		private string _offsetx;
		private string _offsety;
		private int _x;
		private int _y;
		private float _opacity;
		private bool _visible;
		private List<MapProperties> _properties;
		private MapImage _image;

		[XmlElement(ElementName = "properties")]
		public List<MapProperties> Properties
		{
			get => _properties;
			set => _properties = value;
		}

		[XmlElement(ElementName = "image")]
		public MapImage Image
		{
			get => _image;
			set => _image = value;
		}

		[XmlAttribute("name")]
		public string Name
		{
			get => _name;
			set => _name = value;
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
	}
}