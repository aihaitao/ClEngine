using System.Xml.Serialization;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "wangset")]
	public class MapWangset
	{
		private string _name;
		private string _tile;
		private MapWangcornercolor _wangcornercolor;
		private MapWangedgecolor _wangedgecolor;
		private MapWangtile _wangtile;

		[XmlElement(ElementName = "wangcornercolor")]
		public MapWangcornercolor Wangcornercolor
		{
			get => _wangcornercolor;
			set => _wangcornercolor = value;
		}

		[XmlElement(ElementName = "wangedgecolor")]
		public MapWangedgecolor Wangedgecolor
		{
			get => _wangedgecolor;
			set => _wangedgecolor = value;
		}

		[XmlElement(ElementName = "wangtile")]
		public MapWangtile Wangtile
		{
			get => _wangtile;
			set => _wangtile = value;
		}

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
	}
}