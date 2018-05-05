using System;
using GalaSoft.MvvmLight;

namespace ClEngine.CoreLibrary.Asset
{
	[Serializable]
	public class ResourceInfo : ObservableObject
	{
		private string _name;
		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				RaisePropertyChanged(()=>Name);
			}
		}

		private string _path;
		public string Path
		{
			get => _path;
			set
			{
				_path = value;
				RaisePropertyChanged(() => Path);
			}
		}
	}
}