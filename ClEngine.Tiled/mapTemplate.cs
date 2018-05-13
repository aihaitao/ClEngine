using System.Xml.Serialization;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "template")]
	public class MapTemplate
	{
		private MapTileSet _tileSet;
		private MapObject _object;

		[XmlElement(ElementName = "tileset")]
		public MapTileSet Tileset
		{
			get => _tileSet;
			set => _tileSet = value;
		}

		[XmlElement(ElementName = "object")]
		public MapObject Object
		{
			get => _object;
			set => _object = value;
		}
	}
}