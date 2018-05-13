using System.Xml.Serialization;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "wangsets")]
	public class MapWangsets
	{
		private MapWangset _wangset;

		[XmlElement(ElementName = "wangset")]
		public MapWangset Wangset
		{
			get => _wangset;
			set => _wangset = value;
		}
	}
}