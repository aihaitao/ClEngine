using System.Xml.Serialization;

namespace ClEngine.CoreLibrary.Map.MapEnum
{
	public enum MapCompression
	{
		[XmlEnum("gzip")]
		Gzip,
		[XmlEnum("zlib")]
		Zlib,
	}
}