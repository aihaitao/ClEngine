using System.Xml.Serialization;

namespace ClEngine.CoreLibrary.Map
{
	/// <remarks/>
	[System.Serializable, XmlRoot(ElementName = "mapGroupObjectgroupObjectPolyline")]
	public partial class MapGroupObjectgroupObjectPolyline
	{

		private string _pointsField;

		[XmlAttribute("points")]
		public string Points
		{
			get => _pointsField;
			set => _pointsField = value;
		}
	}
}