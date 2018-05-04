using System.Windows.Controls;
using ClEngine.View.Map;
using ClEngine.ViewModel;

namespace ClEngine
{
    /// <summary>
    /// MapEditor.xaml 的交互逻辑
    /// </summary>
    public partial class MapEditor : UserControl
    {
        private MapEditorViewModel _mapEditorViewModel;
        public MapEditor()
        {
            InitializeComponent();

            _mapEditorViewModel = new MapEditorViewModel();
            DataContext = _mapEditorViewModel;
            MapEventDataGrid.ItemsSource = _mapEditorViewModel.Model;

			CreateContextMenu();
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
