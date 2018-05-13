using System.Collections.Generic;
using System.Xml.Serialization;
using ClEngine.Tiled.MapEnum;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "data")]
	public class MapData
	{
		private MapEncoding _encoding;
		private MapCompression _compression;
		private List<MapTile> _tile;
		private MapChunk _chunk;
		private string _value;

		[XmlAttribute("encoding")]
		public MapEncoding Encoding
		{
			get => _encoding;
			set => _encoding = value;
		}

		[XmlAttribute("compression")]
		public MapCompression Compression
		{
			get => _compression;
			set => _compression = value;
		}

		[XmlElement(ElementName = "tile")]
		public List<MapTile> Tile
		{
			get => _tile;
			set => _tile = value;
		}

		[XmlElement(ElementName = "chunk")]
		public MapChunk Chunk
		{
			get => _chunk;
			set => _chunk = value;
		}

		[XmlText]
		public string Value
		{
			get => _value;
			set => _value = value;
		}
	}
}