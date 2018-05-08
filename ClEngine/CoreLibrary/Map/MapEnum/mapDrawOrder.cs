using System.Xml.Serialization;

namespace ClEngine.CoreLibrary.Map.MapEnum
{
	public enum MapDrawOrder
	{
		[XmlEnum("index")]
		Index,
		[XmlEnum("topdown")]
		TopDown,
	}
}