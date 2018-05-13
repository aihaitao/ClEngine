using System.Xml.Serialization;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "tileoffset")]
	public class MapTileOffset
	{
		private string _x;
		private string _y;

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
	}
}