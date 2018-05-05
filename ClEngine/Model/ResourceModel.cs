using System;
using System.Collections.Generic;
using ClEngine.CoreLibrary.Asset;
using GalaSoft.MvvmLight;

namespace ClEngine.Model
{
	[Serializable]
	public class ResourceModel : ObservableObject
	{
		private string _name;
		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				RaisePropertyChanged(() => Name);
			}
		}

		private ResourceType _type;
		public ResourceType Type
		{
			get => _type;
			set
			{
				_type = value;
				RaisePropertyChanged(() => Type);
			}
		}

		private List<ResourceInfo> _resources;
		public List<ResourceInfo> Resources
		{
			get => _resources;
			set
			{
				_resources = value;
				RaisePropertyChanged(() => Resources);
			}
		}

		public ResourceModel()
		{
			Resources = new List<ResourceInfo>();
		}
	}
}