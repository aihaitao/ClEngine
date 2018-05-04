using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ClEngine.Model;
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
			var logModel = new LogModel {LogLevel = LogLevel.Error};

			if (string.IsNullOrEmpty(MapName.Text))
			{
				logModel.Message = "地图名称不能为空";
				Messenger.Default.Send(logModel, "Log");

				return;
			}

			if (!int.TryParse(LogicGridHeight.Text, out int logicGridHeight))
			{
				logModel.Message = "逻辑格子高度必须为数值";
				Messenger.Default.Send(logModel, "Log");

				return;
			}

			if (!int.TryParse(LogicGridWidth.Text, out int logicGridWidth))
			{
				logModel.Message = "逻辑格子宽度必须为数值";
				Messenger.Default.Send(logModel, "Log");

				return;
			}

			if (!int.TryParse(SurfaceGridHeight.Text, out int surfaceGridHeight))
			{
				logModel.Message = "地表格子高度必须为数值";
				Messenger.Default.Send(logModel, "Log");

				return;
			}

			if (!int.TryParse(SurfaceGridWidth.Text, out int surfaceGridWidth))
			{
				logModel.Message = "地表格子宽度必须为数值";
				Messenger.Default.Send(logModel, "Log");

				return;
			}

			if (string.IsNullOrEmpty(ImagePath.Text))
			{
				logModel.Message = "必须选择图片作为地图";
				Messenger.Default.Send(logModel, "Log");

				return;
			}

			MapModel.SurfaceGridHeight = surfaceGridHeight;
			MapModel.SurfaceGridWidth = surfaceGridWidth;
			MapModel.LogicGridWidth = logicGridWidth;
			MapModel.LogicGridHeight = logicGridHeight;
			MapModel.Name = MapName.Text;
			MapModel.ImageName = Path.GetFileNameWithoutExtension(ImagePath.Text);
			
			ImagePath.Text.Compiler();
			
			DialogResult = true;
		}

		private void BrowserMap_OnClick(object sender, RoutedEventArgs e)
		{
			var loadMap = new OpenFileDialog
			{
				CheckFileExists = true,
				CheckPathExists = true,
				Multiselect = false,
				Filter = "图像文件(*.jpg,*.png)|*.jpg;*.png"
			};
			var dialogResult = loadMap.ShowDialog();
			if (dialogResult == true)
			{
				var fileInfo = new FileInfo(loadMap.FileName);
				var mapDir = Path.Combine(MainWindow.ProjectPosition, "Content", "Map");
				var destPosition = Path.Combine(mapDir, fileInfo.Name);
				if (!Directory.Exists(mapDir))
					Directory.CreateDirectory(mapDir);

				File.Copy(loadMap.FileName, destPosition, true);

				ImagePath.Text = destPosition;
			}
		}
	}
}
