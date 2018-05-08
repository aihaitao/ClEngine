using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ClEngine.CoreLibrary.Asset;
using ClEngine.Model;
using ClEngine.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;

namespace ClEngine.View.Asset
{
	/// <summary>
	/// ResourceView.xaml 的交互逻辑
	/// </summary>
	public partial class ResourceView : UserControl
	{
		private ResourceViewModel ResourceViewModel { get; set; }

		public ResourceView()
		{
			InitializeComponent();

			ResourceViewModel = new ResourceViewModel();
			DataContext = ResourceViewModel;

			ResourceTreeView.ItemsSource = ResourceViewModel.ResourceTypeList;
			ResourceTreeView.SelectedItemChanged += ResourceTreeViewOnSelectedItemChanged;
			ResourceDataGrid.SelectionChanged += ResourceDataGridOnSelectionChanged;

			var contextMenu = new ContextMenu();
			var resourceReName = AddItem("重命名", contextMenu);
			var addResource = AddItem("加入资源", contextMenu);
			var removeResource = AddItem("删除资源", contextMenu);

			var separator = new Separator();
			contextMenu.Items.Add(separator);

			var copyName = AddItem("复制名称", contextMenu);

			ResourceDataGrid.ContextMenu = contextMenu;

			addResource.Click += AddResourceOnClick;
		}

		private void ResourceDataGridOnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (ResourceDataGrid.SelectedItem is ResourceInfo resourceInfo)
			{
				var dirName = Path.GetDirectoryName(resourceInfo.Path);
				var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(resourceInfo.Path);
				var resourceReplaceExtension = Path.Combine(dirName ?? throw new InvalidOperationException(),
					fileNameWithoutExtension ?? throw new InvalidOperationException());
				ResourceDraw.LoadResource(resourceReplaceExtension, ((ResourceModel) ResourceTreeView.SelectedItem).Type);
			}
		}

		private void ResourceTreeViewOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (e.NewValue is ResourceModel resourceModel)
			{
				var mgContent = AssetCompilerExtended.GetMgContent();
				var resourceInfo = GetSignResult(mgContent.SourceFiles, resourceModel.Type);
				SetResourceModel(resourceInfo);
			}
		}

		private void AddResourceOnClick(object sender, RoutedEventArgs e)
		{
			var openFileDialog = new OpenFileDialog()
			{
				Multiselect = false,
				CheckFileExists = true,
				CheckPathExists = true,
			};

			var fileExtension = string.Empty;

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

					InitTreeViewResourceModel(resource.Type);
				}
			}
		}

		private void InitTreeViewResourceModel(ResourceType type)
		{
			var sourceFileCollection = AssetCompilerExtended.GetMgContent();
			var resourceInfo = GetSignResult(sourceFileCollection.SourceFiles, type);
			SetResourceModel(resourceInfo);
		}

		private void SetResourceModel(List<ResourceInfo> resourceInfo)
		{
			if (ResourceTreeView.SelectedItem is ResourceModel resource)
			{
				switch (resource.Type)
				{
					case ResourceType.Image:
						ResourceViewModel.SetImageModel(resourceInfo);
						break;
					case ResourceType.Animation:
						ResourceViewModel.SetAnimationModel(resourceInfo);
						break;
					case ResourceType.Sound:
						ResourceViewModel.SetSoundModel(resourceInfo);
						break;
					case ResourceType.Particle:
						ResourceViewModel.SetParticleModel(resourceInfo);
						break;
					case ResourceType.Font:
						ResourceViewModel.SetFontModel(resourceInfo);
						break;
				}
			}
		}

		private List<ResourceInfo> GetSignResult(IEnumerable<string> sourceFiles, ResourceType type)
		{
			var files = new List<ResourceInfo>();
			foreach (var sourceFile in sourceFiles)
			{
				if (sourceFile.Replace(@"/", "\\").Contains(type.ToString().Replace(@"/", "\\")))
				{
					var resourceInfo = new ResourceInfo
					{
						Name = Path.GetFileNameWithoutExtension(sourceFile),
						Path = sourceFile
					};
					files.Add(resourceInfo);
				}
			}

			return files;
		}

		private MenuItem AddItem(string name, ContextMenu contextMenu)
		{
			var menuItem = new MenuItem { Header = name };
			contextMenu.Items.Add(menuItem);

			return menuItem;
		}
	}
}
