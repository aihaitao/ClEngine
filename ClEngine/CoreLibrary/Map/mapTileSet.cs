using System.Xml.Serialization;

namespace ClEngine.CoreLibrary.Map
{
	[System.Serializable, XmlRoot(ElementName = "tileset")]
	public class MapTileSet
	{
		private string _firstgid;
		private string _source;
		private string _name;
		private string _tilewidth;
		private string _tileheight;
		private string _spacing;
		private string _margin;
		private string _tilecount;
		private string _columns;

		[XmlAttribute("firstgid")]
		public string Firstgid
		{
			get => _firstgid;
			set => _firstgid = value;
		}

		[XmlAttribute("source")]
		public string Source
		{
			get => _source;
			set => _source = value;
		}

		[XmlAttribute("name")]
		public string Name
		{
			get => _name;
			set => _name = value;
		}

		[XmlAttribute("tilewidth")]
		public string Tilewidth
		{
			get => _tilewidth;
			set => _tilewidth = value;
		}

		[XmlAttribute("tileheight")]
		public string Tileheight
		{
			get => _tileheight;
			set => _tileheight = value;
		}

		[XmlAttribute("spacing")]
		public string Spacing
		{
			get => _spacing;
			set => _spacing = value;
		}

		[XmlAttribute("margin")]
		public string Margin
		{
			get => _margin;
			set => _margin = value;
		}

		[XmlAttribute("tilecount")]
		public string Tilecount
		{
			get => _tilecount;
			set => _tilecount = value;
		}

		[XmlAttribute("columns")]
		public string Columns
		{
			get => _columns;
			set => _columns = value;
		}
	}
}