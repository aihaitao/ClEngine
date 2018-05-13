using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "object")]
	public class MapObject
	{
		private string _id;
		private string _name;
		private string _type;
		private string _x;
		private string _y;
		private string _width;
		private string _height;
		private string _rotation;
		private string _gid;
		private string _visible;
		private string _template;
		private MapProperties _properties;
		private MapEllipse _ellipse;
		private MapPoint _point;
		private MapPolygon _polygon;
		private MapPolyline _polyline;
		private MapText _text;

		[XmlElement(ElementName = "properties")]
		public MapProperties Properties
		{
			get => _properties;
			set => _properties = value;
		}

		[XmlElement(ElementName = "point")]
		public MapPoint Point
		{
			get => _point;
			set => _point = value;
		}

		[XmlElement(ElementName = "text")]
		public MapText Text
		{
			get => _text;
			set => _text = value;
		}

		[XmlElement(ElementName = "polygon")]
		public MapPolygon Polygon
		{
			get => _polygon;
			set => _polygon = value;
		}

		[XmlElement(ElementName = "polyline")]
		public MapPolyline Polyline
		{
			get => _polyline;
			set => _polyline = value;
		}

		[XmlElement(ElementName = "ellipse")]
		public MapEllipse Ellipse
		{
			get => _ellipse;
			set => _ellipse = value;
		}

		[XmlAttribute("id")]
		public string Id
		{
			get => _id;
			set => _id = value;
		}

		[XmlAttribute("name")]
		public string Name
		{
			get => _name;
			set => _name = value;
		}

		[XmlAttribute("type")]
		public string Type
		{
			get => _type;
			set => _type = value;
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

		[XmlAttribute("rotation")]
		public string Rotation
		{
			get => _rotation;
			set => _rotation = value;
		}

		[XmlAttribute("gid")]
		public string Gid
		{
			get => _gid;
			set => _gid = value;
		}

		[XmlAttribute("visible")]
		public string Visible
		{
			get => _visible;
			set => _visible = value;
		}

		[XmlAttribute("template")]
		public string Template
		{
			get => _template;
			set => _template = value;
		}

		public void Draw(ContentManager content)
		{

		}
	}
}