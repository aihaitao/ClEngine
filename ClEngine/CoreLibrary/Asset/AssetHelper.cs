using System;
using System.Reflection;
using System.Resources;

namespace ClEngine.CoreLibrary.Asset
{
	public static class AssetHelper
	{
		private static readonly Assembly CurrentEntryAssembly = Assembly.GetEntryAssembly();
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

			var resourceManager = new ResourceManager("ClEngine.Properties.Resources", CurrentEntryAssembly);
			var translateName = resourceManager.GetObject(name);
			return translateName != null ? translateName.ToString() : name;
		}
	}
}