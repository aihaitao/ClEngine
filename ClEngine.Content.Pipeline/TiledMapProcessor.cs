using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using ClEngine.Tiled;
using ClEngine.Tiled.MapEnum;
using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Extended;
using MonoGame.Utilities;
using CompressionMode = System.IO.Compression.CompressionMode;

namespace ClEngine.Content.Pipeline
{
	[ContentProcessor(DisplayName = "Tiled Map Processor - ClEnigne")]
	public class TiledMapProcessor : ContentProcessor<Map, Map>
	{
		public override Map Process(Map input, ContentProcessorContext context)
		{
			var previousWorkingDirectory = Environment.CurrentDirectory;
			var newWorkingDirectory = Path.GetDirectoryName(input.FilePath);

			if (string.IsNullOrEmpty(newWorkingDirectory))
				throw new NullReferenceException();

			Environment.CurrentDirectory = newWorkingDirectory;

			foreach (var layer in input.ImageLayer)
			{
				if (layer != null)
				{
					layer.Models = new[] {CreateImageLayerModel(layer) };
				}
			}

			foreach (var mapLayer in input.Layer)
			{
				if (mapLayer != null)
				{
					var data = mapLayer.Data;
					var encodingType = data.Encoding;
					var compressionType = data.Compression;

					var tileData = DecodeTileLayerData(encodingType, mapLayer);
					var tiles = Cre
				}
			}
		}

		private static MapTile[] CreateTiles(RenderOrder renderOrder, int mapWidth, int mapHeight, List<MapTile> tileData)
		{
			MapTile[] tiles;

			switch (renderOrder)
			{
				case RenderOrder.Leftdown:
					tiles = Cre
			}
		}

		private static IEnumerable<MapTile> CreateTilesInLeftDownOrder(List<MapTile> tileLayerData, int mapWidth,
			int mapHeight)
		{
			for (int y = 0; y < mapHeight; y++)
			{
				for (var x = mapWidth - 1; x >= 0; x--)
				{
					var dataIndex = x + y * mapWidth;
					var globalIdentifier = tileLayerData[dataIndex].GlobalIdentifier;
					if (globalIdentifier == 0)
						continue;
					var tile = new MapTile();
					yield return tile;
				}
			}
		}

		private static List<MapTile> DecodeTileLayerData(MapEncoding encodingType, MapLayer tileLayer)
		{
			List<MapTile> tiles;

			switch (encodingType)
			{
				case MapEncoding.Xml:
					tiles = tileLayer.Data.Tile;
					break;
				case MapEncoding.Csv:
					tiles = DecodeCommaSeperatedValueData(tileLayer.Data);
					break;
				case MapEncoding.Base64:
					tiles = DecodeBase64Data(tileLayer.Data, tileLayer.Width, tileLayer.Height);
					break;
				default:
					throw new NotSupportedException($"图层编码{encodingType}不支持");
			}

			return tiles;
		}

		public static List<MapTile> DecodeBase64Data(MapData data, int width, int height)
		{
			var tileList = new List<MapTile>();
			var encodedData = data.Value.Trim();
			var decodedData = Convert.FromBase64String(encodedData);

			using (var stream = OpenStream(decodedData, data.Compression))
			{
				using (var reader = new BinaryReader(stream))
				{
					data.Tile = new List<MapTile>();

					for (int y = 0; y < width; y++)
					{
						for (int x = 0; x < height; x++)
						{
							var gid = reader.ReadUInt32();
							tileList.Add(new MapTile
							{
								GlobalIdentifier = gid
							});
						}
					}
				}
			}

			return tileList;
		}

		private static Stream OpenStream(byte[] decodedData, MapCompression compressionMode)
		{
			var memoryStream = new MemoryStream(decodedData, false);

			switch (compressionMode)
			{
				case MapCompression.Gzip:
					return new GZipStream(memoryStream, CompressionMode.Decompress);
				case MapCompression.Zlib:
					return new ZlibStream(memoryStream, MonoGame.Utilities.CompressionMode.Decompress);
				default:
					return memoryStream;
			}
		}

		private static List<MapTile> DecodeCommaSeperatedValueData(MapData data)
		{
			return data.Value
				.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
				.Select(uint.Parse)
				.Select(x => new MapTile {GlobalIdentifier = x})
				.ToList();
		}

		private static MapLayerModel CreateImageLayerModel(MapImageLayer imageLayer)
		{
			var model = new MapLayerModel(imageLayer.Name, imageLayer.Image);
			model.AddTileIndices();
			model.AddTileVertices(new Point2(imageLayer.X, imageLayer.Y));

			return model;
		}
	}
}