using System.Xml.Serialization;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "tileset")]
	public class MapTileSet
	{
		private string _firstgid;
		private string _source;
		private string _name;
		private int _tilewidth;
		private int _tileheight;
		private int _spacing;
		private int _margin;
		private string _tilecount;
		private int _columns;
		private MapTileOffset _tileOffset;
		private MapGrid _grid;
		private MapProperties _properties;
		private MapImage _image;
		private MapTerraintypes _terraintypes;
		private MapTile _tile;
		private MapWangsets _wangsets;

		[XmlElement(ElementName = "tileoffset")]
		public MapTileOffset Tileoffset
		{
			get => _tileOffset;
			set => _tileOffset = value;
		}

		[XmlElement(ElementName = "grid")]
		public MapGrid Grid
		{
			get => _grid;
			set => _grid = value;
		}

		[XmlElement(ElementName = "image")]
		public MapImage Image
		{
			get => _image;
			set => _image = value;
		}

		[XmlElement(ElementName = "wangsets")]
		public MapWangsets Wangsets
		{
			get => _wangsets;
			set => _wangsets = value;
		}

		[XmlElement(ElementName = "tile")]
		public MapTile Tile
		{
			get => _tile;
			set => _tile = value;
		}

		[XmlElement(ElementName = "properties")]
		public MapProperties Properties
		{
			get => _properties;
			set => _properties = value;
		}

		[XmlElement(ElementName = "terraintypes")]
		public MapTerraintypes Terraintypes
		{
			get => _terraintypes;
			set => _terraintypes = value;
		}

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
		public int Tilewidth
		{
			get => _tilewidth;
			set => _tilewidth = value;
		}

		[XmlAttribute("tileheight")]
		public int Tileheight
		{
			get => _tileheight;
			set => _tileheight = value;
		}

		[XmlAttribute("spacing")]
		public int Spacing
		{
			get => _spacing;
			set => _spacing = value;
		}

		[XmlAttribute("margin")]
		public int Margin
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
		public int Columns
		{
			get => _columns;
			set => _columns = value;
		}
	}
}