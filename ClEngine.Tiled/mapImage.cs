using System.Xml.Serialization;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "image")]
	public class MapImage
	{
		private string _format;
		private string _id;
		private string _source;
		private string _trans;
		private int _width;
		private int _height;
		private MapData _mapData;

		[XmlElement(ElementName = "mapData")]
		public MapData MapData
		{
			get => _mapData;
			set => _mapData = value;
		}

		[XmlAttribute("format")]
		public string Format
		{
			get => _format;
			set => _format = value;
		}

		[XmlAttribute("id")]
		public string Id
		{
			get => _id;
			set => _id = value;
		}

		[XmlAttribute("source")]
		public string Source
		{
			get => _source;
			set => _source = value;
		}

		[XmlAttribute("trans")]
		public string Trans
		{
			get => _trans;
			set => _trans = value;
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
	}
}