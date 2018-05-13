using System.Collections.Generic;
using System.Xml.Serialization;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "animation")]
	public class MapAnimation
	{
		private List<MapFrame> _frame;

		[XmlElement(ElementName = "frame")]
		public List<MapFrame> Frame
		{
			get => _frame;
			set => _frame = value;
		}
	}
}