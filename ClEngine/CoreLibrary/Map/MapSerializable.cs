using System.Collections.Generic;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;

namespace ClEngine.CoreLibrary.Map
{
	[System.Serializable, XmlRoot(ElementName = "clmap")]
	public class MapSerializable : ObservableObject
	{
		private List<MapInfo> _mapInfos;
		public List<MapInfo> MapList
		{
			get => _mapInfos;
			set
			{
				_mapInfos = value;
				RaisePropertyChanged(() => MapList);
			}
		}

		public MapSerializable()
		{
			MapList = new List<MapInfo>();
		}
	}
}