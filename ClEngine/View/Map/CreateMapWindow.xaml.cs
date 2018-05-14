using System;
using System.IO;
using System.Windows;
using ClEngine.CoreLibrary.Asset;
using ClEngine.CoreLibrary.Map;
using ClEngine.Model;
using ClEngine.Tiled.MapEnum;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;

namespace ClEngine.View.Map
{
	/// <summary>
	/// CreateMapWindow.xaml 的交互逻辑
	/// </summary>
	public partial class CreateMapWindow : Window
	{
		private MapModel MapModel { get; set; }
		public CreateMapWindow()
		{
			InitializeComponent();

			MapModel = new MapModel();
			DataContext = MapModel;
		}

		private void CreateMap_OnClick(object sender, RoutedEventArgs e)
		{
			if (Equals(MapTabControl.SelectedItem, SelfTabItem))
			{
				Tiled.Map map = new Tiled.Map();
				int.TryParse(FixedX.Text, out var width);
				int.TryParse(FixedY.Text, out var height);
				int.TryParse(BlockWidth.Text, out var blockWidth);
				int.TryParse(BlockHeight.Text, out var blockHeight);
				Enum.TryParse(MapDirection.SelectedValue.ToString(), out MapOrientation orientation);
				Enum.TryParse(RenderOrder.SelectedValue.ToString(), out RenderOrder renderOrder);
				map.Width = width;
				map.Height = height;
				map.Tilewidth = blockWidth;
				map.Tileheight = blockHeight;
				map.Orientation = orientation;
				map.Renderorder = renderOrder;


				// TODO: Create Map
			}
			else if (Equals(MapTabControl.SelectedItem, MapTabItem))
			{
				var surfaceGridHeight = 0;
				var surfaceGridWidth = 0;
				var logicGridWidth = 0;
				var logicGridHeight = 0;
				if (!CheckMapTabItem(ref surfaceGridHeight, ref surfaceGridWidth, ref logicGridWidth, ref logicGridHeight))
					return;

				MapModel.SurfaceGridHeight = surfaceGridHeight;
				MapModel.SurfaceGridWidth = surfaceGridWidth;
				MapModel.LogicGridWidth = logicGridWidth;
				MapModel.LogicGridHeight = logicGridHeight;
				MapModel.Name = MapName.Text;
				MapModel.ImageName = Path.GetFileNameWithoutExtension(ImagePath.Text);

				ImagePath.Text.Compiler(ResourceType.Map);
			}

			DialogResult = true;
		}

		private bool CheckMapTabItem(ref int logicGridHeight, ref int logicGridWidth, ref int surfaceGridHeight,
			ref int surfaceGridWidth)
		{
			var logModel = new LogModel {LogLevel = LogLevel.Error};

			if (string.IsNullOrEmpty(MapName.Text))
			{
				logModel.Message = "地图名称不能为空";
				Messenger.Default.Send(logModel, "Log");

				return false;
			}

			if (!int.TryParse(LogicGridHeight.Text, out logicGridHeight))
			{
				logModel.Message = "逻辑格子高度必须为数值";
				Messenger.Default.Send(logModel, "Log");

				return false;
			}

			if (!int.TryParse(LogicGridWidth.Text, out logicGridWidth))
			{
				logModel.Message = "逻辑格子宽度必须为数值";
				Messenger.Default.Send(logModel, "Log");

				return false;
			}

			if (!int.TryParse(SurfaceGridHeight.Text, out surfaceGridHeight))
			{
				logModel.Message = "地表格子高度必须为数值";
				Messenger.Default.Send(logModel, "Log");

				return false;
			}

			if (!int.TryParse(SurfaceGridWidth.Text, out surfaceGridWidth))
			{
				logModel.Message = "地表格子宽度必须为数值";
				Messenger.Default.Send(logModel, "Log");

				return false;
			}

			if (string.IsNullOrEmpty(ImagePath.Text))
			{
				logModel.Message = "必须选择图片作为地图";
				Messenger.Default.Send(logModel, "Log");

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
