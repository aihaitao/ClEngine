using System.Xml.Serialization;

namespace ClEngine.CoreLibrary.Map
{
	[System.Serializable, XmlRoot(ElementName = "properties")]
	public class MapProperties
	{
		private MapProperty _mapProperty;

		[XmlElement(ElementName = "property")]
		public MapProperty MapProperty
		{
			get => _mapProperty;
			set => _mapProperty = value;
		}
	}
}