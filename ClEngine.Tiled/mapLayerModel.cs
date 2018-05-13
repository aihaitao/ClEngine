using System.Collections.Generic;
using System.IO;
using ClEngine.Tiled.MapEnum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace ClEngine.Tiled
{
	public class MapLayerModel
	{
		private List<VertexPositionTexture> _vertices;
		private List<ushort> _indices;

		public string LayerName { get; set; }
		public List<VertexPositionTexture> Vertices { get; set; }
		public List<ushort> Indices { get; set; }
		public Size2 ImageSize { get; set; }
		public string TextureAssetName { get; set; }

		public MapLayerModel(string layerName, MapImage image)
		{
			LayerName = layerName;
			_vertices = new List<VertexPositionTexture>();
			Vertices = new List<VertexPositionTexture>(_vertices);
			Indices = new List<ushort>();
			ImageSize = new Size2(image.Width, image.Height);
			TextureAssetName = Path.ChangeExtension(image.Source, null);
		}

		public MapLayerModel(string layerName, MapTileSet tileSet)
			: this(layerName, tileSet.Image)
		{
		}

		public void AddTileVertices(Point2 position, Rectangle? sourceRectangle = null,
			MapTileFlipFlags flags = MapTileFlipFlags.None)
		{
			float texelLeft, texelTop, texelRight, texelBottom;
			var sourceRectanglel = sourceRectangle ?? new Rectangle(0, 0, (int) ImageSize.Width, (int) ImageSize.Height);

			if (sourceRectangle.HasValue)
			{
				var reciprocalWidth = 1f / ImageSize.Width;
				var reciprocalHeight = 1f / ImageSize.Height;
				texelLeft = sourceRectanglel.X * reciprocalWidth;
				texelTop = sourceRectanglel.Y * reciprocalHeight;
				texelRight = (sourceRectanglel.X + sourceRectanglel.Width) * reciprocalWidth;
				texelBottom = (sourceRectanglel.Y + sourceRectanglel.Height) * reciprocalHeight;
			}
			else
			{
				texelLeft = 0;
				texelTop = 0;
				texelBottom = 1;
				texelRight = 1;
			}

			VertexPositionTexture vertexTopLeft, vertexTopRight, vertexBottomLeft, vertexBottomRight;

			vertexTopLeft.Position = new Vector3(position, 0);
			vertexTopRight.Position = new Vector3(position + new Vector2(sourceRectanglel.Width, 0), 0);
			vertexBottomLeft.Position = new Vector3(position + new Vector2(0, sourceRectanglel.Height), 0);
			vertexBottomRight.Position = new Vector3(position + new Vector2(sourceRectanglel.Width, sourceRectanglel.Height), 0);

			vertexTopLeft.TextureCoordinate.Y = texelTop;
			vertexTopLeft.TextureCoordinate.X = texelLeft;

			vertexTopRight.TextureCoordinate.Y = texelTop;
			vertexTopRight.TextureCoordinate.X = texelRight;

			vertexBottomLeft.TextureCoordinate.Y = texelBottom;
			vertexBottomLeft.TextureCoordinate.X = texelLeft;

			vertexBottomRight.TextureCoordinate.Y = texelBottom;
			vertexBottomRight.TextureCoordinate.X = texelRight;

			var flipDiagonally = (flags & MapTileFlipFlags.FlipDiagonally) != 0;
			var flipHorizontally = (flags & MapTileFlipFlags.FlipHorizontally) != 0;
			var flipVertically = (flags & MapTileFlipFlags.FlipVertically) != 0;

			if (flipDiagonally)
			{
				FloatHelper.Swap(ref vertexTopRight.TextureCoordinate.X, ref vertexBottomLeft.TextureCoordinate.X);
				FloatHelper.Swap(ref vertexTopRight.TextureCoordinate.Y, ref vertexBottomLeft.TextureCoordinate.Y);
			}

			if (flipHorizontally)
			{
				if (flipDiagonally)
				{
					FloatHelper.Swap(ref vertexTopLeft.TextureCoordinate.Y, ref vertexTopRight.TextureCoordinate.Y);
					FloatHelper.Swap(ref vertexBottomLeft.TextureCoordinate.Y, ref vertexBottomRight.TextureCoordinate.Y);
				}
				else
				{
					FloatHelper.Swap(ref vertexTopLeft.TextureCoordinate.X, ref vertexTopRight.TextureCoordinate.X);
					FloatHelper.Swap(ref vertexBottomLeft.TextureCoordinate.X, ref vertexBottomRight.TextureCoordinate.X);
				}
			}

			if (flipVertically)
			{
				if (flipDiagonally)
				{
					FloatHelper.Swap(ref vertexTopLeft.TextureCoordinate.X, ref vertexBottomLeft.TextureCoordinate.X);
					FloatHelper.Swap(ref vertexTopRight.TextureCoordinate.X, ref vertexBottomRight.TextureCoordinate.X);
				}
				else
				{
					FloatHelper.Swap(ref vertexTopLeft.TextureCoordinate.Y, ref vertexBottomLeft.TextureCoordinate.Y);
					FloatHelper.Swap(ref vertexTopRight.TextureCoordinate.Y, ref vertexBottomRight.TextureCoordinate.Y);
				}
			}

			_vertices.Add(vertexTopLeft);
			_vertices.Add(vertexTopRight);
			_vertices.Add(vertexBottomLeft);
			_vertices.Add(vertexBottomRight);
		}

		public void AddTileIndices()
		{
			var indexOffset = Vertices.Count;

			_indices.Add((ushort)(0 + indexOffset));
			_indices.Add((ushort)(1 + indexOffset));
			_indices.Add((ushort)(2 + indexOffset));
			_indices.Add((ushort)(1 + indexOffset));
			_indices.Add((ushort)(3 + indexOffset));
			_indices.Add((ushort)(2 + indexOffset));
		}
	}
}