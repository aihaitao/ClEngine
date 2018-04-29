using System;
using System.ComponentModel;
using System.Windows.Controls;
using ClEngine.Model;
using ClEngine.ViewModel;
using Microsoft.Win32;

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
        }

        public void LoadMap()
        {
            var loadMap = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                Filter = "图像文件(*.jpg,*.png)|*.jpg;*.png"
            };
            loadMap.FileOk += LoadMapOnFileOk;
            loadMap.ShowDialog();
        }

        private void LoadMapOnFileOk(object sender, CancelEventArgs cancelEventArgs)
        {
            
        }
    }
}
