using System.Xml.Serialization;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "wangedgecolor")]
	public class MapWangedgecolor
	{
		private string _name;
		private string _color;
		private string _tile;
		private string _probability;

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

		[XmlAttribute("tile")]
		public string Tile
		{
			get => _tile;
			set => _tile = value;
		}

		[XmlAttribute("probability")]
		public string Probability
		{
			get => _probability;
			set => _probability = value;
		}
	}
}