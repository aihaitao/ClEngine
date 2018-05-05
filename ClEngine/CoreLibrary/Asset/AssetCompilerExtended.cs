using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using ClEngine.Model;
using GalaSoft.MvvmLight.Messaging;

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
		/// 开始编译
		/// </summary>
		/// <param name="assetPath">资源路径</param>
		/// <param name="type">资源类型</param>
		public static void Compiler(this string assetPath, ResourceType type = ResourceType.Unknown)
		{
			if (type == ResourceType.Particle)
				return;

			var sourceFileCollection = GetMgContent();

			AssetPath = assetPath;
			var resourceCompilerDir = Path.Combine(Environment.CurrentDirectory, "runtime", "assetcompiler");
			var resourceCompilerPath = Path.Combine(resourceCompilerDir, ResourceCompiler);
			var arguments = GetArguments(assetPath, sourceFileCollection);
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

				process.ErrorDataReceived += ProcessOnErrorDataReceived;
				process.OutputDataReceived += ProcessOnOutputDataReceived;
				process.BeginErrorReadLine();
				process.BeginOutputReadLine();
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
		/// 获取文件夹中间名
		/// </summary>
		/// <param name="assetPath"></param>
		/// <param name="replacePath"></param>
		/// <returns></returns>
		private static string GetIntermediateName(string assetPath, string replacePath)
		{
			var assetDir = Path.GetDirectoryName(assetPath);
			return assetDir?.Replace(string.Concat(replacePath, "\\"), "");
		}

		/// <summary>
		/// 获取完整资源输出目录
		/// </summary>
		/// <param name="headerPath"></param>
		/// <param name="intermediatePath"></param>
		/// <returns></returns>
		private static string GetFullContainsDirName(string headerPath, string intermediatePath)
		{
			return Path.Combine(headerPath, intermediatePath);
		}

		/// <summary>
		/// 获取参数
		/// </summary>
		/// <param name="assetPath"></param>
		/// <param name="sourceFileCollection"></param>
		/// <returns></returns>
		private static string GetArguments(string assetPath, SourceFileCollection sourceFileCollection)
		{
			if (!Directory.Exists(Intermediate))
				Directory.CreateDirectory(Intermediate);

			var arguments = string.Empty;
			arguments += GetBuildArgument(sourceFileCollection, assetPath);								// 源文件目录
			arguments += string.Concat(" /outputDir:", Content);										// 编译文件输出目录
			arguments += string.Concat(" /intermediateDir:", Intermediate);								// 中间文件输出目录
			arguments += string.Concat(" /compress:", "true");											// 资源将压缩
			arguments += string.Concat(" /reference:",						
				Path.Combine(Environment.CurrentDirectory, "MonoGame.Extended.Content.Pipeline.dll"));	// 引用第三方库
			return arguments;
		}

		/// <summary>
		/// 获取编译文件参数
		/// </summary>
		/// <param name="sourceFileCollection">历史编译文件</param>
		/// <param name="assetPath">当前需要编译文件</param>
		/// <returns></returns>
		private static string GetBuildArgument(SourceFileCollection sourceFileCollection, string assetPath)
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

			arguments += string.Concat(" /build:", assetPath);

			return arguments;
		}

		private static void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			var logModel = new LogModel
			{
				Message = e.Data,
				LogLevel = LogLevel.Log
			};
			Messenger.Default.Send(logModel, "Log");
		}

		private static void ProcessOnErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			var logModel = new LogModel
			{
				Message = e.Data,
				LogLevel = LogLevel.Error
			};
			Messenger.Default.Send(logModel, "Log");
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

			if (!sourcePath.Equals(destPosition))
				File.Copy(sourcePath, destPosition, true);

			return destPosition;
		}
	}
}