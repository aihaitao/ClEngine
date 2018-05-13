using System.Xml.Serialization;
using ClEngine.Tiled.MapEnum;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "property")]
	public class MapProperty
	{
		private string _name;
		private MapPropertyType _type;
		private string _value;

		[XmlAttribute("name")]
		public string Name
		{
			get => _name;
			set => _name = value;
		}

		[XmlAttribute("type")]
		public MapPropertyType Type
		{
			get => _type;
			set => _type = value;
		}

		[XmlAttribute("value")]
		public string Value
		{
			get => _value;
			set => _value = value;
		}
	}
}