using System.Xml.Serialization;

namespace ClEngine.CoreLibrary.Map
{
	/// <remarks/>
	[System.Serializable, XmlRoot(ElementName = "mapGroupImagelayerImage")]
	public partial class MapGroupImagelayerImage
	{

		private string _sourceField;

		private ushort _widthField;

		private ushort _heightField;

		[XmlAttribute("source")]
		public string Source
		{
			get => _sourceField;
			set => _sourceField = value;
		}

		[XmlAttribute("width")]
		public ushort Width
		{
			get => _widthField;
			set => _widthField = value;
		}

		[XmlAttribute("height")]
		public ushort Height
		{
			get => _heightField;
			set => _heightField = value;
		}
	}
}