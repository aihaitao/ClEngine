using System.Collections.Generic;
using System.Reflection;
using ClEngine.CoreLibrary.Asset;
using GalaSoft.MvvmLight;

namespace ClEngine.ViewModel
{
	public class ResourceViewModel : ViewModelBase
	{
		public readonly List<AssetResolver> ResourceTypeList = new List<AssetResolver>();

		public ResourceViewModel()
		{
			InitResourceType();
		}

		private void InitResourceType()
		{
			ResourceTypeList.Clear();
			var assembly = Assembly.GetExecutingAssembly();
			foreach (var type in assembly.GetTypes())
			{
				if (type.BaseType == typeof(AssetResolver) && type.IsAbstract == false)
				{
					var resolver = (AssetResolver) assembly.CreateInstance(type.FullName, true);
					if (resolver != null && resolver.UseBundle)
						ResourceTypeList.Add(resolver);
				}
			}
		}
	}
}