using System.Xml.Serialization;

namespace ClEngine.Tiled.MapEnum
{
	public enum MapCompression
	{
		[XmlEnum("xml")]
		Xml,
		[XmlEnum("gzip")]
		Gzip,
		[XmlEnum("zlib")]
		Zlib,
	}
}