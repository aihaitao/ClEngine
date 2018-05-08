using System.Xml.Serialization;

namespace ClEngine.CoreLibrary.Map.MapEnum
{
	public enum MapPropertyType
	{
		[XmlEnum("string")]
		String,
		[XmlEnum("int")]
		Int,
		[XmlEnum("float")]
		Float,
		[XmlEnum("bool")]
		Bool,
		[XmlEnum("color")]
		Color,
		[XmlEnum("file")]
		File,
	}
}