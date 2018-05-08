using System.Xml.Serialization;

namespace ClEngine.CoreLibrary.Map
{
	/// <remarks/>
	[System.Serializable, XmlRoot(ElementName = "mapGroupLayer")]
	public partial class MapGroupLayer
	{

		private MapGroupLayerData _dataField;

		private string _nameField;

		private byte _widthField;

		private byte _heightField;

		[XmlElement(ElementName = "data")]
		public MapGroupLayerData Data
		{
			get => _dataField;
			set => _dataField = value;
		}

		[XmlAttribute("name")]
		public string Name
		{
			get => _nameField;
			set => _nameField = value;
		}

		[XmlAttribute("width")]
		public byte Width
		{
			get => _widthField;
			set => _widthField = value;
		}

		[XmlAttribute("height")]
		public byte Height
		{
			get => _heightField;
			set => _heightField = value;
		}
	}
}