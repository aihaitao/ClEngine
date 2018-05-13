// 注意: 生成的代码可能至少需要 .NET Framework 4.5 或 .NET Core/Standard 2.0。

using System.Collections.Generic;
using System.Xml.Serialization;
using ClEngine.Tiled.MapEnum;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "map")]
	public class Map
	{
		private MapGroup _groupField;
		private List<MapTileSet> _tileset;
		private MapProperties _properties;
		private RenderOrder _renderorderField;
		private decimal _versionField;
		private string _tiledversionField;
		private MapOrientation _orientationField;
		private int _widthField;
		private int _heightField;
		private int _tilewidthField;
		private int _tileheightField;
		private byte _infiniteField;
		private byte _nextobjectidField;
		private string _hexsidelength;
		private string _staggeraxis;
		private string _staggerindex;
		private string _backgroundcolor;
		private List<MapLayer> _layer;
		private List<MapImageLayer> _imageLayer;

		[XmlElement(ElementName = "group")]
		public MapGroup Group
		{
			get => _groupField;
			set => _groupField = value;
		}

		[XmlElement(ElementName = "imagelayer")]
		public List<MapImageLayer> ImageLayer
		{
			get => _imageLayer;
			set => _imageLayer = value;
		}

		[XmlElement(ElementName = "tileset")]
		public List<MapTileSet> TileSet
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
		public List<MapLayer> Layer
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
		public MapOrientation Orientation
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
		public int Width
		{
			get => _widthField;
			set => _widthField = value;
		}
		
		[XmlAttribute("height")]
		public int Height
		{
			get => _heightField;
			set => _heightField = value;
		}

		[XmlAttribute("tilewidth")]
		public int Tilewidth
		{
			get => _tilewidthField;
			set => _tilewidthField = value;
		}

		[XmlAttribute("tileheight")]
		public int Tileheight
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

		[XmlElement(ElementName = "backgroundcolor")]
		public string Backgroundcolor
		{
			get => _backgroundcolor;
			set => _backgroundcolor = value;
		}
	}
}