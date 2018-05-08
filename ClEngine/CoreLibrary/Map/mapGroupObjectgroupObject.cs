using System.Xml.Serialization;

namespace ClEngine.CoreLibrary.Map
{
	/// <remarks/>
	[System.Serializable, XmlRoot(ElementName = "mapGroupObjectgroupObject")]
	public partial class MapGroupObjectgroupObject
	{

		private MapGroupObjectgroupObjectPolyline _polylineField;

		private byte _idField;

		private string _nameField;

		private string _typeField;

		private decimal _xField;

		private decimal _yField;

		[XmlElement(ElementName = "polyline")]
		public MapGroupObjectgroupObjectPolyline Polyline
		{
			get => _polylineField;
			set => _polylineField = value;
		}

		[XmlAttribute("id")]
		public byte Id
		{
			get => _idField;
			set => _idField = value;
		}

		[XmlAttribute("name")]
		public string Name
		{
			get => _nameField;
			set => _nameField = value;
		}

		[XmlAttribute("type")]
		public string Type
		{
			get => _typeField;
			set => _typeField = value;
		}

		[XmlAttribute("x")]
		public decimal X
		{
			get => _xField;
			set => _xField = value;
		}

		[XmlAttribute("y")]
		public decimal Y
		{
			get => _yField;
			set => _yField = value;
		}
	}
}