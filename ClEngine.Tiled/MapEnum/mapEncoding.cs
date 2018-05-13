using System.Xml.Serialization;

namespace ClEngine.Tiled.MapEnum
{
	public enum MapEncoding
	{
		[XmlEnum("base64")]
		Base64,
		[XmlEnum("csv")]
		Csv,
	}
}