using System.Windows;
using ClEngine.CoreLibrary.Asset;
using ClEngine.Model;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;

namespace ClEngine.View.Asset
{
	public partial class ResourceView
	{
		private void AddResourceOnClick(object sender, RoutedEventArgs e)
		{
			var openFileDialog = new OpenFileDialog()
			{
				Multiselect = false,
				CheckFileExists = true,
				CheckPathExists = true,
			};

			if (ResourceTreeView.SelectedItem == null)
			{
				var logModel = new LogModel
				{
					LogLevel = LogLevel.Error,
					Message = "清先选择要加入的资源类型"
				};
				Messenger.Default.Send(logModel, "Log");

				return;
			}

			if (ResourceTreeView.SelectedItem is ResourceModel resource)
			{
				string fileExtension;
				switch (resource.Type)
				{
					case ResourceType.Image:
						fileExtension = "图片资源 (*.bmp, *.png, *.jpg)|*.bmp;*.png;*.jpg";
						break;
					case ResourceType.Animation:
						fileExtension = "动画资源 (*.ani, *.png)|*.ani;*.png";
						break;
					case ResourceType.Sound:
						fileExtension = "声音资源 (*.wav, *.mp3)|*.wav;*.mp3";
						break;
					case ResourceType.Particle:
						fileExtension = "粒子资源 (*.em, *.xml)|*.em;*.xml";
						break;
					case ResourceType.Font:
						fileExtension = "字体资源 (*.spritefont, *.fnt, *.png)|*.spritefont;*.fnt;*.png";
						break;
					default:
						fileExtension = "所有资源 (*.*)|*.*";
						break;
				}

				openFileDialog.Filter = fileExtension;
				if (openFileDialog.ShowDialog() == true)
				{
					openFileDialog.FileName.Compiler(resource.Type);
				}
			}
		}
	}
}