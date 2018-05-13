using System.IO;
using System.Windows.Controls;
using ClEngine.CoreLibrary.Map;
using ClEngine.Tiled;
using ClEngine.View.Map;
using ClEngine.ViewModel;

namespace ClEngine
{
    /// <summary>
    /// MapEditor.xaml 的交互逻辑
    /// </summary>
    public partial class MapEditor : UserControl
    {
        public static MapEditorViewModel MapEditorViewModel;
		public MapEditor()
        {
			InitializeComponent();

			MapEditorViewModel = new MapEditorViewModel(MapListView);
            DataContext = MapEditorViewModel;

			MapListView.SelectionChanged += MapListViewOnSelectionChanged;

			CreateContextMenu();
        }

	    private void MapListViewOnSelectionChanged(object sender, SelectionChangedEventArgs e)
	    {
		    if (MapListView.SelectedItem is MapInfo info)
		    {
			    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(info.Source);
			    var fileDir = Path.GetDirectoryName(info.Source);
			    var result = Path.Combine(fileDir, fileNameWithoutExtension);
			    MapEventDataGrid.ItemsSource = MapHelper.GetMap(info.Source).Group.Objectgroup;
				MapDraw.Instance.SetCurrentMap(info.Source);
		    }
	    }

	    private void CreateContextMenu()
	    {
			var contextMenu = new ContextMenu();
			var textBlock = new MenuItem { Header = "新建地图" };
			textBlock.Click += delegate { CreateMap(); };

			contextMenu.Items.Add(textBlock);
			

		    MapListView.ContextMenu = contextMenu;
	    }

	    private void CreateMap()
        {
			var createMap = new CreateMapWindow();
			createMap.ShowDialog();
        }
    }
}
