using System.Xml.Serialization;
using ClEngine.CoreLibrary.Map.MapEnum;

namespace ClEngine.CoreLibrary.Map
{
	[System.Serializable, XmlRoot(ElementName = "data")]
	public class MapData
	{
		private MapEncoding _encoding;
		private MapCompression _compression;

		[XmlAttribute("encoding")]
		public MapEncoding Encoding
		{
			get => _encoding;
			set => _encoding = value;
		}

		[XmlAttribute("compression")]
		public MapCompression Compression
		{
			get => _compression;
			set => _compression = value;
		}
	}
}