using System.Xml.Serialization;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "terraintypes")]
	public class MapTerraintypes
	{
		private MapTerrain _mapTerrain;

		[XmlElement(ElementName = "mapTerrain")]
		public MapTerrain MapTerrain
		{
			get => _mapTerrain;
			set => _mapTerrain = value;
		}
	}
}