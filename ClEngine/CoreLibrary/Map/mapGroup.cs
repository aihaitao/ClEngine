using System.Xml.Serialization;

namespace ClEngine.CoreLibrary.Map
{
	[System.Serializable, XmlRoot(ElementName = "mapGroup")]
	public partial class MapGroup
	{
		private MapGroupImagelayer _imagelayerField;
		private MapGroupLayer _layerField;
		private MapGroupObjectgroup _objectgroupField;
		private string _nameField;

		[XmlElement(ElementName = "imagelayer")]
		public MapGroupImagelayer Imagelayer
		{
			get => _imagelayerField;
			set => _imagelayerField = value;
		}

		[XmlElement(ElementName = "layer")]
		public MapGroupLayer Layer
		{
			get => _layerField;
			set => _layerField = value;
		}

		[XmlElement(ElementName = "objectgroup")]
		public MapGroupObjectgroup Objectgroup
		{
			get => _objectgroupField;
			set => _objectgroupField = value;
		}

		[XmlAttribute("name")]
		public string Name
		{
			get => _nameField;
			set => _nameField = value;
		}
	}
}