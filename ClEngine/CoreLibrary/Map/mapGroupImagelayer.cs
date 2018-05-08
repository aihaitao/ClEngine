using System.Xml.Serialization;

namespace ClEngine.CoreLibrary.Map
{
	/// <remarks/>
	[System.Serializable, XmlRoot(ElementName = "mapGroupImagelayer")]
	public partial class MapGroupImagelayer
	{

		private MapGroupImagelayerImage _imageField;

		private string _nameField;

		private byte _lockedField;

		[XmlElement(ElementName = "image")]
		public MapGroupImagelayerImage Image
		{
			get => _imageField;
			set => _imageField = value;
		}

		[XmlAttribute("name")]
		public string Name
		{
			get => _nameField;
			set => _nameField = value;
		}

		[XmlAttribute("locked")]
		public byte Locked
		{
			get => _lockedField;
			set => _lockedField = value;
		}
	}
}