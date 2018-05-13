using System.Xml.Serialization;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "wangtile")]
	public class MapWangtile
	{
		private string _tileid;
		private string _wangid;

		[XmlAttribute("tileid")]
		public string Tileid
		{
			get => _tileid;
			set => _tileid = value;
		}

		[XmlAttribute("wangid")]
		public string Wangid
		{
			get => _wangid;
			set => _wangid = value;
		}
	}
}