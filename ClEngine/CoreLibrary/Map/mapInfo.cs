using System.Xml.Serialization;
using GalaSoft.MvvmLight;

namespace ClEngine.CoreLibrary.Map
{
	[System.Serializable, XmlRoot(ElementName = "info")]
	public class MapInfo : ObservableObject
	{
		private string _source;
		private string _name;

		[XmlAttribute("source")]
		public string Source
		{
			get => _source;
			set
			{
				_source = value;
				RaisePropertyChanged(() => Source);
			}
		}
		[XmlAttribute("name")]
		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				RaisePropertyChanged(() => Name);
			}
		}
	}
}