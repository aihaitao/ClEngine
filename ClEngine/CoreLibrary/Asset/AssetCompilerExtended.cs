using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using ClEngine.CoreLibrary.Map;

namespace ClEngine.CoreLibrary.Asset
{
	public static class AssetCompilerExtended
	{
		/// <summary>
		/// 资源编译器名称
		/// </summary>
		private const string ResourceCompiler = "MGCB.exe";
		/// <summary>
		/// 资源路径
		/// </summary>
		private static string AssetPath { get; set; }
		private static ResourceType AssetType { get; set; }
		private static string CompiledAssetPath { get; set; }

		/// <summary>
		/// 源文件目录
		/// 临时放置于Content 让源文件与编译文件处在一起
		/// </summary>
		public static readonly string SourceContent = Path.Combine(MainWindow.ProjectPosition, "Content");

		/// <summary>
		/// 编译目录
		/// 临时放置于Content 让源文件与编译文件处在一起
		/// TODO： 编译目录始终与源文件在一起 需要进程结束后 引擎进行源文件复制/移动
		/// BUG: 只移动会导致下次编译重新进行 因为找不到编译文件
		/// </summary>
		public static readonly string Content = Path.Combine(MainWindow.ProjectPosition, "Content");

		/// <summary>
		/// 中间目录
		/// </summary>
		public static readonly string Intermediate = Path.Combine(MainWindow.ProjectPosition, "Intermediate");

		/// <summary>
		/// 地图源目录
		/// </summary>
		public static readonly string MapSourceContent = Path.Combine(SourceContent, "Map");

		public static readonly string MapSourceManage = "map.cl";
		public static readonly string ImageSourceContent = Path.Combine(SourceContent, "Image");


		/// <summary>
		/// 图片资源标识
		/// </summary>
		public const string ImageSign = "Image";
		/// <summary>
		/// 动画资源标识
		/// </summary>
		public const string AnimationSign = "Animation";
		/// <summary>
		/// 声音资源标识
		/// </summary>
		public const string SoundSign = "Sound";
		/// <summary>
		/// 粒子资源标识
		/// </summary>
		public const string ParticleSign = "Particle";
		/// <summary>
		/// 字体资源标识
		/// </summary>
		public const string FontSign = "Font";
		/// <summary>
		/// 地图资源标识
		/// </summary>
		public const string MapSign = "Map";

		/// <summary>
		/// 开始编译
		/// </summary>
		/// <param name="assetPath">资源路径</param>
		/// <param name="type">资源类型</param>
		public static void Compiler(this string assetPath, ResourceType type = ResourceType.Unknown)
		{
			// 判断资源类型
			if (type == ResourceType.Particle)	// 粒子资源不进行编译处理
				return;

			AssetPath = assetPath;
			AssetType = type;

			if (type == ResourceType.Map)
				InitTileMapResource(assetPath);

			var resourceName = string.Empty;
			switch (type)
			{
				case ResourceType.Image:
					resourceName = Path.Combine(SourceContent, ImageSign);
					break;
				case ResourceType.Animation:
					resourceName = Path.Combine(SourceContent, AnimationSign);
					break;
				case ResourceType.Sound:
					resourceName = Path.Combine(SourceContent, SoundSign);
					break;
				case ResourceType.Particle:
					resourceName = Path.Combine(SourceContent, ParticleSign);
					break;
				case ResourceType.Font:
					resourceName = Path.Combine(SourceContent, FontSign);
					break;
				case ResourceType.Map:
					resourceName = Path.Combine(SourceContent, MapSign);
					break;
				case ResourceType.Unknown:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}

			// 移动原始资源至工程目录下
			var projectAsset = resourceName.InitAsset(assetPath);
			
			// 寻找资源编译器
			var sourceFileCollection = GetMgContent();

			var resourceCompilerDir = Path.Combine(Environment.CurrentDirectory, "runtime", "assetcompiler");
			var resourceCompilerPath = Path.Combine(resourceCompilerDir, ResourceCompiler);
			var arguments = GetArguments(projectAsset, sourceFileCollection, type);
			// 设置编译参数
			var startInfo = new ProcessStartInfo(resourceCompilerPath)
			{
				CreateNoWindow = true,
				WorkingDirectory = resourceCompilerDir,
				RedirectStandardError = true,
				RedirectStandardOutput = true,
				Arguments = arguments,
				UseShellExecute = false,
			};
			var process = Process.Start(startInfo);
			if (process != null)
			{
				process.EnableRaisingEvents = true;
				process.Exited += ProcessOnExited;
			    process.ErrorDataReceived += (sender, args) => Logger.Logger.Error(args.Data);
			    process.OutputDataReceived += (sender, args) => Logger.Logger.Log(args.Data);
				process.BeginErrorReadLine();
				process.BeginOutputReadLine();
			}
		}

		private static void ProcessOnExited(object sender, EventArgs e)
		{
			if (AssetType == ResourceType.Map)
			{
				if (!string.IsNullOrEmpty(CompiledAssetPath))
				{
					var mapInfo = new MapInfo
					{
						Source = CompiledAssetPath,
						Name = Path.GetFileNameWithoutExtension(CompiledAssetPath),
					};

					MapEditor.MapEditorViewModel.Serializable.MapList.Add(mapInfo);
					var mapSourceManage = Path.Combine(MapSourceContent, MapSourceManage);

					using (var fileStream = new FileStream(mapSourceManage, FileMode.OpenOrCreate))
					{
						var serializer = new XmlSerializer(typeof(MapSerializable));
						serializer.Serialize(fileStream, MapEditor.MapEditorViewModel.Serializable);
					}
				}
			}
		}

		private static void InitTileMapResource(string assetPath)
		{
			Tiled.Map map;
			using (var fileStream = new FileStream(assetPath, FileMode.Open))
			{
				var xmlSerilizer = new XmlSerializer(typeof(Tiled.Map));
				map = (Tiled.Map)xmlSerilizer.Deserialize(fileStream);
				foreach (var imageLayer in map.Group.Imagelayer)
				{
					var source = imageLayer.Image.Source;
					if (!string.IsNullOrEmpty(source))
					{
						var sourceDir = Path.GetDirectoryName(assetPath);
						var imageFullName = Path.Combine(sourceDir, source);
						var destDir = Path.Combine(ImageSourceContent, Path.GetFileName(source));
						File.Copy(imageFullName, destDir, true);
					}

					imageLayer.Image.Source = Path.Combine("..", ImageSign, Path.GetFileName(source));
				}
			}

			CompiledAssetPath =  Path.Combine(SourceContent, MapSign).InitAsset(assetPath);
			SaveTileMapResource(CompiledAssetPath, map);
		}

		private static void SaveTileMapResource(string assetPath, object type)
		{
			using (var fileStream = new FileStream(assetPath, FileMode.Open))
			{
				var xmlSerilizer = new XmlSerializer(typeof(Tiled.Map));
				xmlSerilizer.Serialize(fileStream, type);
			}
		}

		public static SourceFileCollection GetMgContent()
		{
			var intermediateFile = Path.Combine(Intermediate, ".mgcontent");
			var sourceFileCollection = new SourceFileCollection();
			if (File.Exists(intermediateFile))
			{
				using (var stream = new FileStream(intermediateFile, FileMode.Open))
				{
					var xmldes = new XmlSerializer(typeof(SourceFileCollection));
					sourceFileCollection = (SourceFileCollection)xmldes.Deserialize(stream);
				}
			}

			return sourceFileCollection;
		}

		/// <summary>
		/// 获取参数
		/// </summary>
		/// <param name="assetPath"></param>
		/// <param name="sourceFileCollection"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		private static string GetArguments(string assetPath, SourceFileCollection sourceFileCollection, ResourceType type)
		{
			if (!Directory.Exists(Intermediate))
				Directory.CreateDirectory(Intermediate);

			var arguments = string.Empty;
			arguments += GetBuildArgument(sourceFileCollection, assetPath, type);						// 源文件目录
			arguments += string.Concat(" /outputDir:", Content);										// 编译文件输出目录
			arguments += string.Concat(" /intermediateDir:", Intermediate);								// 中间文件输出目录
			arguments += string.Concat(" /compress:", "true");											// 资源将压缩
			arguments += string.Concat(" /reference:",
				Path.Combine(Environment.CurrentDirectory, "MonoGame.Extended.Content.Pipeline.dll"));  // 引用第三方库
			return arguments;
		}

		/// <summary>
		/// 获取编译文件参数
		/// </summary>
		/// <param name="sourceFileCollection">历史编译文件</param>
		/// <param name="assetPath">当前需要编译文件</param>
		/// <param name="type"></param>
		/// <returns></returns>
		private static string GetBuildArgument(SourceFileCollection sourceFileCollection, string assetPath, ResourceType type)
		{
			var arguments = string.Empty;
			if (sourceFileCollection?.SourceFiles != null)
			{
				foreach (var sourceFile in sourceFileCollection.SourceFiles)
				{
					if (!sourceFile.Equals(assetPath))
						arguments += string.Concat(" /build:", sourceFile);
				}
			}

			if (type == ResourceType.Map)
			{
				using (var fileStream = new FileStream(assetPath, FileMode.Open))
				{
					var serializer = new XmlSerializer(typeof(Tiled.Map));
					var mapDeserialize = (Tiled.Map)serializer.Deserialize(fileStream);
					foreach (var mapImageLayer in mapDeserialize.Group.Imagelayer)
					{
						if (!string.IsNullOrEmpty(mapImageLayer.Image.Source))
							arguments += string.Concat(" /build:", Path.Combine(MapSourceContent, mapImageLayer.Image.Source));
					}
				}
			}

			arguments += string.Concat(" /build:", assetPath);

			return arguments;
		}
        
		/// <summary>
		/// 初始化资源
		/// </summary>
		/// <param name="destPath"></param>
		/// <param name="sourcePath"></param>
		/// <returns></returns>
		public static string InitAsset(this string destPath, string sourcePath)
		{
			var fileInfo = new FileInfo(sourcePath);
			var destPosition = Path.Combine(destPath, fileInfo.Name);
			if (!Directory.Exists(destPath))
				Directory.CreateDirectory(destPath);

			if (!sourcePath.Equals(destPosition) && !File.Exists(destPosition))
				File.Copy(sourcePath, destPosition);

			return destPosition;
		}
	}
}