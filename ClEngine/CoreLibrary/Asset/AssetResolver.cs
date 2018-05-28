using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Serialization;
using ClEngine.CoreLibrary.Editor;

namespace ClEngine.CoreLibrary.Asset
{

    [Serializable]
	public abstract class AssetResolver: ICompiler, ISerializable
	{
		public string Name { get; set; }
		
		public abstract string Extension { get; }

		public ObservableCollection<ResourceInfo> Resolvers { get; private set; }

		[NonSerialized]
		protected readonly AssetTranslator Translator;
		
		public string StoragePath => Path.Combine(SourceAsset, Name);
		
		public string Arguments { get; private set; }

        private static string Intermediate => EditorRecord.MainViewModel.ProjectPosition != null
			? Path.Combine(EditorRecord.MainViewModel.ProjectPosition, "Intermediate")
			: "Intermediate";

		public static string SourceAsset =>
		    EditorRecord.MainViewModel.ProjectPosition != null ? Path.Combine(EditorRecord.MainViewModel.ProjectPosition, Content) : Content;
		
		private static string Content => "Content";
		[NonSerialized]
		private string _originPath;

		public string XnaAssetPath { get; private set; }

        /// <summary>
        /// 是否使用资源管理
        /// 默认:<value>true</value>
        /// </summary>
        [NonSerialized] public bool UseBundle;

		protected AssetResolver(string name)
		{
			Name = name;
			UseBundle = true;
		    Type = SerializeType.Xml;

            Translator = AssetTranslator.GetTranslator();
		    Translator.Complete = UpdateResolver;
            Resolvers = new ObservableCollection<ResourceInfo>();
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

            Translator.Compiler(Arguments);

			XnaAssetPath = _originPath;
		    UpdateResolver();
		}

	    public void UpdateResolver()
	    {
	        Application.Current.Dispatcher.Invoke(() =>
	        {
	            Resolvers.Clear();

	            if (!Directory.Exists(StoragePath))
	                Directory.CreateDirectory(StoragePath);

	            var files = Directory.GetFiles(StoragePath, "*.xnb", SearchOption.AllDirectories);
	            foreach (var file in files)
	                Resolvers.Add(new ResourceInfo {Name = Path.GetFileNameWithoutExtension(file), Path = file});
	        });
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
            if (!Directory.Exists(Intermediate))
		        Directory.CreateDirectory(Intermediate);

		    Arguments = $"/build:{_originPath} /outputDir:{StoragePath} /intermediateDir:{Intermediate} /compress:true";

		    SetReference();
        }

		private void SetReference()
		{
			var monoGameExtended = Path.Combine(EditorRecord.EditorEnvironment, "MonoGame.Extended.Content.Pipeline.dll");
			Arguments += $" /reference:{monoGameExtended}";
		}

		protected virtual void SetOtherCompilerArugments()
		{
		}

	    public virtual string SerializerName => Path.Combine(StoragePath,
	        string.Concat(Path.GetFileNameWithoutExtension(new FileInfo(XnaAssetPath).Name), ".asset"));

	    public void Serialize(object type)
	    {
	        if (Type == SerializeType.Xml)
	            SerializeToXml(type);

	    }

	    private void SerializeToXml(object type)
	    {
            var xmlSerializer = new XmlSerializer(type.GetType());
	        using (var fileStream = new FileStream(SerializerName, FileMode.OpenOrCreate))
	        {
	            xmlSerializer.Serialize(fileStream, type);
	        }
	    }

	    public object DeSerialize()
	    {
	        throw new NotImplementedException();
	    }

	    public SerializeType Type { get; set; }
	}
}