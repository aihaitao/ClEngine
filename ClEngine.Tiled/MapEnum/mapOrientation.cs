using System.Xml.Serialization;

namespace ClEngine.Tiled.MapEnum
{
	public enum MapOrientation
	{
		[XmlEnum("orthogonal")]
		Orthogonal,
		[XmlEnum("isometric")]
		Isometric,
		[XmlEnum("staggered")]
		Staggered,
		[XmlEnum("hexagonal")]
		Hexagonal,
	}
}