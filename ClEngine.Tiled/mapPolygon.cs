using System.Xml.Serialization;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "polygon")]
	public class MapPolygon
	{
		private string _points;

		[XmlAttribute("points")]
		public string Points
		{
			get => _points;
			set => _points = value;
		}
	}
}