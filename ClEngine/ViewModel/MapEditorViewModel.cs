using System.IO;
using System.Windows.Controls;
using System.Xml.Serialization;
using ClEngine.CoreLibrary.Asset;
using ClEngine.CoreLibrary.Map;
using GalaSoft.MvvmLight;

namespace ClEngine.ViewModel
{
    public class MapEditorViewModel : ViewModelBase
    {
	    private MapSerializable _serializable;
        public MapResolver MapResolver { get; set; }
		public MapSerializable Serializable
		{
			get => _serializable;
			set
			{
				_serializable = value;
				RaisePropertyChanged(() => Serializable);
			}
		}
		private readonly ListView _listView;
		public MapEditorViewModel(ListView listView)
		{
			_listView = listView;
			Serializable = new MapSerializable();
		    MapResolver = new MapResolver();
        }

	    public void LoadMapList()
	    {
		    var mapSourceManage = Path.Combine(MapResolver.StoragePath, MapResolver.MapManage);

		    if (!File.Exists(mapSourceManage))
			    return;

		    using (var fileStream = new FileStream(mapSourceManage, FileMode.Open))
		    {
			    var serializer = new XmlSerializer(typeof(MapSerializable));
			    Serializable = (MapSerializable)serializer.Deserialize(fileStream);

			    if (_listView.ItemsSource != null)
				    _listView.ItemsSource = null;

			    _listView.ItemsSource = Serializable.MapList;
		    }
	    }
	}
}