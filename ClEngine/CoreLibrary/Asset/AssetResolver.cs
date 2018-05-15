using System;
using System.IO;
using System.Xml.Serialization;

namespace ClEngine.CoreLibrary.Asset
{
	public abstract class AssetResolver : ICompiler
	{
		public string Name { get; set; }
		protected string StoragePath => Path.Combine(SourceAsset, Name);
		private string Arguments { get; set; }

		private static string Intermediate => MainWindow.ProjectPosition != null
			? Path.Combine(MainWindow.ProjectPosition, "Intermediate")
			: "Intermediate";

		private static string SourceAsset =>
			MainWindow.ProjectPosition != null ? Path.Combine(MainWindow.ProjectPosition, Content) : Content;

		private static string Content => "Content";
		private string _originPath;

		protected AssetResolver(string name)
		{
			Name = name;
		}

		/// <summary>
		/// 开始编译
		/// </summary>
		/// <param name="path"></param>
		public virtual void Compiler(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				throw new ArgumentNullException($"编译资源路径不能为空:{nameof(path)}");

			_originPath = path;
			
			MoveAsset(ref _originPath);
			GetBuildArgument(_originPath, GetMgContent());
			SetDefaultCompilerArguments();
			SetOtherCompilerArugments();
			CompileAsset();
		}

		/// <summary>
		/// 移动资源
		/// </summary>
		/// <param name="orginPath"></param>
		protected virtual void MoveAsset(ref string orginPath)
		{
			var fileInfo = new FileInfo(orginPath);
			var destPosition = Path.Combine(StoragePath, fileInfo.Name);
			if (!Directory.Exists(StoragePath))
				Directory.CreateDirectory(StoragePath);

			if (!orginPath.Equals(destPosition) && !File.Exists(destPosition))
				File.Copy(orginPath, destPosition);

			orginPath = destPosition;
		}

		/// <summary>
		/// 获取编译参数
		/// </summary>
		/// <param name="path"></param>
		/// <param name="sourceFileCollection"></param>
		private void GetBuildArgument(string path, SourceFileCollection sourceFileCollection)
		{
			if (sourceFileCollection?.SourceFiles != null)
			{
				foreach (var sourceFile in sourceFileCollection.SourceFiles)
				{
					if (!sourceFile.Equals(path))
					{
						Arguments += string.Concat(" /build:", sourceFile);
					}
				}
			}
		}

		/// <summary>
		/// 获取MgContent文件
		/// </summary>
		/// <returns></returns>
		private SourceFileCollection GetMgContent()
		{
			var intermediateContent = Path.Combine(Intermediate, ".mgcontent");
			var sourceFileCollection = new SourceFileCollection();
			if (File.Exists(intermediateContent))
			{
				using (var stream = new FileStream(intermediateContent, FileMode.Open))
				{
					var xmldes = new XmlSerializer(typeof(SourceFileCollection));
					sourceFileCollection = (SourceFileCollection)xmldes.Deserialize(stream);
				}
			}

			return sourceFileCollection;
		}

		/// <summary>
		/// 默认编译资源
		/// 输出文件夹、中间文件、是否压缩、引用文件
		/// </summary>
		protected virtual void CompileAsset()
		{

		}

		protected virtual void SetDefaultCompilerArguments()
		{
			SetOutputDir();
			SetIntermediate();
			SetCompress();
			SetReference();
		}

		private void SetOutputDir()
		{
			Arguments += string.Concat(" /outputDir:", SourceAsset);
		}

		private void SetIntermediate()
		{
			Arguments += string.Concat(" /intermediateDir:", Intermediate);
		}

		private void SetCompress()
		{
			Arguments += string.Concat(" /compress", "true");
		}

		private void SetReference()
		{
			var monoGameExtended = Path.Combine(EditorRecord.EditorEnvironment, "MonoGame.Extended.Content.Pipeline.dll");
			Arguments += string.Concat(" /reference:", monoGameExtended);
		}

		protected virtual void SetOtherCompilerArugments()
		{
		}
	}
}