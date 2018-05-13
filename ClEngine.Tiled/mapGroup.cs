using System.Collections.Generic;
using System.Xml.Serialization;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "group")]
	public class MapGroup
	{
		private List<MapImageLayer> _imagelayer;
		private List<MapLayer> _layer;
		private List<MapObjectGroup> _objectgroup;
		private string _name;
		private string _offsetx;
		private string _offsety;
		private string _opacity;
		private string _visible;
		private MapProperties _properties;
		private MapGroup _group;

		[XmlElement(ElementName = "imagelayer")]
		public List<MapImageLayer> Imagelayer
		{
			get => _imagelayer;
			set => _imagelayer = value;
		}

		[XmlElement(ElementName = "group")]
		public MapGroup Group
		{
			get => _group;
			set => _group = value;
		}

		[XmlElement(ElementName = "properties")]
		public MapProperties Properties
		{
			get => _properties;
			set => _properties = value;
		}

		[XmlElement(ElementName = "layer")]
		public List<MapLayer> Layer
		{
			get => _layer;
			set => _layer = value;
		}

		[XmlElement(ElementName = "objectgroup")]
		public List<MapObjectGroup> Objectgroup
		{
			get => _objectgroup;
			set => _objectgroup = value;
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
	}
}