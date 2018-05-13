using System.Collections.Generic;
using System.Xml.Serialization;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "tile")]
	public class MapTile
	{
		private string _id;
		private string _type;
		private string _terrain;
		private string _probability;
		private MapProperties _properties;
		private MapImage _image;
		private MapObjectGroup _objectGroup;
		private List<MapAnimation> _animation;

		[XmlAttribute("id")]
		public string Id
		{
			get => _id;
			set => _id = value;
		}

		[XmlAttribute("type")]
		public string Type
		{
			get => _type;
			set => _type = value;
		}

		[XmlAttribute("terrain")]
		public string Terrain
		{
			get => _terrain;
			set => _terrain = value;
		}

		[XmlAttribute("probability")]
		public string Probability
		{
			get => _probability;
			set => _probability = value;
		}

		[XmlElement(ElementName = "properties")]
		public MapProperties Properties
		{
			get => _properties;
			set => _properties = value;
		}

		[XmlElement(ElementName = "image")]
		public MapImage Image
		{
			get => _image;
			set => _image = value;
		}

		[XmlElement(ElementName = "objectGroup")]
		public MapObjectGroup ObjectGroup
		{
			get => _objectGroup;
			set => _objectGroup = value;
		}

		[XmlElement(ElementName = "animation")]
		public List<MapAnimation> Animation
		{
			get => _animation;
			set => _animation = value;
		}

		[XmlAttribute("gid")]
		public uint GlobalIdentifier;
	}
}