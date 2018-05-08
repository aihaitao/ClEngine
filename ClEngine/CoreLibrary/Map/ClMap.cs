// 注意: 生成的代码可能至少需要 .NET Framework 4.5 或 .NET Core/Standard 2.0。

using System.Xml.Serialization;
using ClEngine.CoreLibrary.Map.MapEnum;

namespace ClEngine.CoreLibrary.Map
{
	[System.Serializable, XmlRoot(ElementName = "map")]
	public class Map
	{
		private MapGroup _groupField;
		private MapTileSet _tileset;
		private MapProperties _properties;
		private RenderOrder _renderorderField;
		private decimal _versionField;
		private string _tiledversionField;
		private string _orientationField;
		private byte _widthField;
		private byte _heightField;
		private byte _tilewidthField;
		private byte _tileheightField;
		private byte _infiniteField;
		private byte _nextobjectidField;
		private string _hexsidelength;
		private string _staggeraxis;
		private string _staggerindex;
		private string _backgroundcolor;
		private MapLayer _layer;

		[XmlElement(ElementName = "group")]
		public MapGroup Group
		{
			get => _groupField;
			set => _groupField = value;
		}

		[XmlElement(ElementName = "tileset")]
		public MapTileSet TileSet
		{
			get => _tileset;
			set => _tileset = value;
		}

		[XmlElement(ElementName = "properties")]
		public MapProperties Properties
		{
			get => _properties;
			set => _properties = value;
		}

		[XmlElement(ElementName = "layer")]
		public MapLayer Layer
		{
			get => _layer;
			set => _layer = value;
		}

		[XmlAttribute("version")]
		public decimal Version
		{
			get => _versionField;
			set => _versionField = value;
		}
		
		[XmlAttribute("tiledversion")]
		public string TiledVersion
		{
			get => _tiledversionField;
			set => _tiledversionField = value;
		}
		
		[XmlAttribute("orientation")]
		public string Orientation
		{
			get => _orientationField;
			set => _orientationField = value;
		}
		
		[XmlAttribute("renderorder")]
		public RenderOrder Renderorder
		{
			get => _renderorderField;
			set => _renderorderField = value;
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

		[XmlAttribute("tilewidth")]
		public byte Tilewidth
		{
			get => _tilewidthField;
			set => _tilewidthField = value;
		}

		[XmlAttribute("tileheight")]
		public byte Tileheight
		{
			get => _tileheightField;
			set => _tileheightField = value;
		}

		[XmlAttribute("infinite")]
		public byte Infinite
		{
			get => _infiniteField;
			set => _infiniteField = value;
		}
		

		[XmlAttribute("nextobjectid")]
		public byte Nextobjectid
		{
			get => _nextobjectidField;
			set => _nextobjectidField = value;
		}

		[XmlAttribute("hexsidelength")]
		public string Hexsidelength
		{
			get => _hexsidelength;
			set => _hexsidelength = value;
		}

		[XmlAttribute("staggeraxis")]
		public string Staggeraxis
		{
			get => _staggeraxis;
			set => _staggeraxis = value;
		}

		[XmlAttribute("staggerindex")]
		public string Staggerindex
		{
			get => _staggerindex;
			set => _staggerindex = value;
		}

		[XmlAttribute("backgroundcolor")]
		public string Backgroundcolor
		{
			get => _backgroundcolor;
			set => _backgroundcolor = value;
		}
	}
}