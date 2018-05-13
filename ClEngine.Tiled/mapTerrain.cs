using System.Xml.Serialization;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "terrain")]
	public class MapTerrain
	{
		private string _name;
		private string _tile;
		private MapProperties _properties;

		[XmlAttribute("name")]
		public string Name
		{
			get => _name;
			set => _name = value;
		}

		[XmlAttribute("tile")]
		public string Tile
		{
			get => _tile;
			set => _tile = value;
		}

		[XmlElement(ElementName = "properties")]
		public MapProperties Properties
		{
			get => _properties;
			set => _properties = value;
		}
	}
}