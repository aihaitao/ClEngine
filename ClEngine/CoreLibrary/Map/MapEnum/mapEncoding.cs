using System.Xml.Serialization;

namespace ClEngine.CoreLibrary.Map.MapEnum
{
	public enum MapEncoding
	{
		[XmlEnum("base64")]
		Base64,
		[XmlEnum("csv")]
		Csv,
	}
}