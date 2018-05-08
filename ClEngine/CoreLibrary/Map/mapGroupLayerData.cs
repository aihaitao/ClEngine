using System.Xml.Serialization;

namespace ClEngine.CoreLibrary.Map
{
	/// <remarks/>
	[System.Serializable, XmlRoot(ElementName = "mapGroupLayerData")]
	public partial class MapGroupLayerData
	{

		private string _encodingField;

		private string _compressionField;

		private string _valueField;

		[XmlAttribute("encoding")]
		public string Encoding
		{
			get => _encodingField;
			set => _encodingField = value;
		}

		[XmlAttribute("compression")]
		public string Compression
		{
			get => _compressionField;
			set => _compressionField = value;
		}
		
		[XmlText]
		public string Value
		{
			get => _valueField;
			set => _valueField = value;
		}
	}
}