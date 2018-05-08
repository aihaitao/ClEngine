using System.Xml.Serialization;

namespace ClEngine.CoreLibrary.Map.MapEnum
{
	public enum RenderOrder
	{
		[XmlEnum("right-down")]
		Rightdown,
		[XmlEnum("right-up")]
		Rightup,
		[XmlEnum("left-down")]
		Leftdown,
		[XmlEnum("left-up")]
		Leftup,
	}
}