using System.Xml.Serialization;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "chunk")]
	public class MapChunk
	{
		private string _x;
		private string _y;
		private string _height;
		private string _width;
		private MapTile _tile;

		[XmlElement(ElementName = "tile")]
		public MapTile Tile
		{
			get => _tile;
			set => _tile = value;
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

		[XmlAttribute("height")]
		public string Height
		{
			get => _height;
			set => _height = value;
		}

		[XmlAttribute("width")]
		public string Width
		{
			get => _width;
			set => _width = value;
		}
	}
}