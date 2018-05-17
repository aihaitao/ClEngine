using System.IO;
using System.Resources;
using ClEngine.Properties;

namespace ClEngine.CoreLibrary.Asset
{
	public static class AssetHelper
	{
		private static readonly FileSystemWatcher FileSystemWatcher = new FileSystemWatcher();
		public static dynamic GetTranslateName(this object obj)
		{
			if (obj is string name)
			{
				return BeginTranslateName(name);
			}

			return obj;
		}

		private static string BeginTranslateName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				return string.Empty;

			var resourceManager = new ResourceManager("ClEngine.Properties.Resources", typeof(Resources).Assembly);
			var translateName = resourceManager.GetObject(name);
			return translateName != null ? translateName.ToString() : name;
		}

		public static FileSystemWatcher GetFileSystemWatcher(string path)
		{
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			FileSystemWatcher.IncludeSubdirectories = true;
			FileSystemWatcher.Path = path;
			FileSystemWatcher.EnableRaisingEvents = true;

			return FileSystemWatcher;
		}
	}
}