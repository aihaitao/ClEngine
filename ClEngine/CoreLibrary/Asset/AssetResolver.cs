using System;
using System.IO;

namespace ClEngine.CoreLibrary.Asset
{
	public abstract class AssetResolver : ICompiler
	{
		private string Name { get; set; }
		protected string StoragePath => Path.Combine(SourceAsset, Name);
		
		private static string SourceAsset =>
			MainWindow.ProjectPosition != null ? Path.Combine(MainWindow.ProjectPosition, Content) : Content;

		private static string Content => "Content";
		private string _originPath;

		protected AssetResolver(string name)
		{
			Name = name;
		}

		public virtual void Compiler(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				throw new ArgumentNullException($"编译资源路径不能为空:{nameof(path)}");

			_originPath = path;

			MoveAsset(ref _originPath);
			CompileAsset();
		}

		protected abstract void MoveAsset(ref string orginPath);

		protected abstract void CompileAsset();
	}
}