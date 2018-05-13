using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using ClEngine.Tiled;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace ClEngine.Content.Pipeline
{
	[ContentImporter(".tmx", DefaultProcessor = "TiledMapProcessor", DisplayName = "Tiled Map Importer - ClEngine")]
	public class TiledMapImporter : ContentImporter<Map>
	{
		public override Map Import(string filename, ContentImporterContext context)
		{
			if (filename == null)
				throw new ArgumentNullException(nameof(filename));

			var map = DeserializerTiledMapContent(filename);

			if (map.Width > ushort.MaxValue || map.Height > ushort.MaxValue)
				throw new InvalidContentException($"地图 {filename} 太大. 最大支持的宽度与高度不能超过 {ushort.MaxValue}");

			return map;
		}

		private static Map DeserializerTiledMapContent(string mapFilePath)
		{
			using (var reader = new StreamReader(mapFilePath))
			{
				var mapSerializer = new XmlSerializer(typeof(Map));
				var map = (Map) mapSerializer.Deserialize(reader);

				map.FilePath = mapFilePath;

				var tilesetSerializer = new XmlSerializer(typeof(MapTileSet));

				for (int i = 0; i < map.TileSet.Count; i++)
				{
					var tileset = map.TileSet[i];

					if (!string.IsNullOrWhiteSpace(tileset.Source))
					{
						var tilesetFilePath = GetTilesetFilePath(mapFilePath, tileset);
						map.TileSet[i] = ImportTileset(tilesetFilePath, tilesetSerializer, tileset);
					}
				}

				map.Name = mapFilePath;
				return map;
			}
		}

		private static string GetTilesetFilePath(string mapFilePath, MapTileSet tileset)
		{
			var directoryName = Path.GetDirectoryName(mapFilePath);
			Debug.Assert(directoryName != null, "directoryName != null");

			var tilesetLocation = tileset.Source.Replace('/', Path.DirectorySeparatorChar);
			var tilesetFilePath = Path.Combine(directoryName, tilesetLocation);
			return tilesetFilePath;
		}

		private static MapTileSet ImportTileset(string tilesetFilePath, XmlSerializer tilesetSerializer, MapTileSet tileSet)
		{
			MapTileSet result;

			using (var file = new FileStream(tilesetFilePath, FileMode.Open))
			{
				var importTileset = (MapTileSet) tilesetSerializer.Deserialize(file);
				importTileset.Firstgid = tileSet.Firstgid;
				result = importTileset;
			}

			return result;
		}
	}
}