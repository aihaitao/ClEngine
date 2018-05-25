using System.Windows;
using ClEngine.CoreLibrary.Logger;
using ClEngine.ViewModel;
using Microsoft.Win32;

namespace ClEngine.View.Map
{
	/// <summary>
	/// CreateMapWindow.xaml 的交互逻辑
	/// </summary>
	public partial class CreateMapWindow : Window
	{
		public CreateMapWindow()
		{
            InitializeComponent();
        }

		private bool CheckMapTabItem(ref int logicGridHeight, ref int logicGridWidth, ref int surfaceGridHeight,
			ref int surfaceGridWidth)
		{
			if (string.IsNullOrEmpty(MapName.Text))
			{
                Logger.Error("地图名称不能为空");
				return false;
			}

			if (!int.TryParse(LogicGridHeight.Text, out logicGridHeight))
			{
			    Logger.Error("逻辑格子高度必须为数值");
                return false;
			}

			if (!int.TryParse(LogicGridWidth.Text, out logicGridWidth))
			{
			    Logger.Error("逻辑格子宽度必须为数值");

				return false;
			}

			if (!int.TryParse(SurfaceGridHeight.Text, out surfaceGridHeight))
			{
			    Logger.Error("地表格子高度必须为数值");

				return false;
			}

			if (!int.TryParse(SurfaceGridWidth.Text, out surfaceGridWidth))
			{
			    Logger.Error("地表格子宽度必须为数值");

				return false;
			}

			if (string.IsNullOrEmpty(ImagePath.Text))
			{
			    Logger.Error("必须选择图片作为地图");

				return false;
			}

			return true;
		}

		private void BrowserMap_OnClick(object sender, RoutedEventArgs e)
		{
			var loadMap = new OpenFileDialog
			{
				CheckFileExists = true,
				CheckPathExists = true,
				Multiselect = false,
				Filter = "地图文件(*.tmx, *.xml, *.json)|*.tmx;*.xml;*.json"
			};
			var dialogResult = loadMap.ShowDialog();
			if (dialogResult == true)
			{
				ImagePath.Text = loadMap.FileName;
			}
		}
	}
}
