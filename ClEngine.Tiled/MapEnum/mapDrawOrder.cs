using System.Xml.Serialization;

namespace ClEngine.Tiled.MapEnum
{
	public enum MapDrawOrder
	{
		[XmlEnum("index")]
		Index,
		[XmlEnum("topdown")]
		TopDown,
	}
}