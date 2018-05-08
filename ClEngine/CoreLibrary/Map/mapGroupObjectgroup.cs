using System.Xml.Serialization;

namespace ClEngine.CoreLibrary.Map
{
	/// <remarks/>
	[System.Serializable, XmlRoot(ElementName = "mapGroupObjectgroup")]
	public partial class MapGroupObjectgroup
	{

		private MapGroupObjectgroupObject _objectField;

		private string _nameField;

		[XmlElement(ElementName = "@object")]
		public MapGroupObjectgroupObject Object
		{
			get => _objectField;
			set => _objectField = value;
		}

		[XmlAttribute("name")]
		public string Name
		{
			get => _nameField;
			set => _nameField = value;
		}
	}
}