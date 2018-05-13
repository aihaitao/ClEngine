using System.Xml.Serialization;
using ClEngine.Tiled.MapEnum;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "grid")]
	public class MapGrid
	{
		private MapOrientation _orientation;
		private string _width;
		private string _height;

		[XmlAttribute("orientation")]
		public MapOrientation Orientation
		{
			get => _orientation;
			set => _orientation = value;
		}

		[XmlAttribute("width")]
		public string Width
		{
			get => _width;
			set => _width = value;
		}

		[XmlAttribute("height")]
		public string Height
		{
			get => _height;
			set => _height = value;
		}
	}
}