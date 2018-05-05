using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
		private const string ImageSign = "Image";
		private const string AnimationSign = "Animation";
		private const string SoundSign = "Sound";
		private const string ParticleSign = "Particle";
		private const string FontSign = "Font";

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
				var resourceName = string.Empty;
				switch (resourceModel.Type)
				{
					case ResourceType.Image:
						resourceName = Path.Combine(AssetCompilerExtended.SourceContent, ImageSign);
						break;
					case ResourceType.Animation:
						resourceName = Path.Combine(AssetCompilerExtended.SourceContent, AnimationSign);
						break;
					case ResourceType.Sound:
						resourceName = Path.Combine(AssetCompilerExtended.SourceContent, SoundSign);
						break;
					case ResourceType.Particle:
						resourceName = Path.Combine(AssetCompilerExtended.SourceContent, ParticleSign);
						break;
					case ResourceType.Font:
						resourceName = Path.Combine(AssetCompilerExtended.SourceContent, FontSign);
						break;
				}
				var resourceInfo = GetSignResult(mgContent.SourceFiles, resourceName);
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
			var resourceName = string.Empty;

			if (ResourceTreeView.SelectedItem == null)
			{
				var logModel = new LogModel()
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
						resourceName = Path.Combine(AssetCompilerExtended.SourceContent, ImageSign);
						break;
					case ResourceType.Animation:
						fileExtension = "动画资源 (*.ani, *.png)|*.ani;*.png";
						resourceName = Path.Combine(AssetCompilerExtended.SourceContent, AnimationSign);
						break;
					case ResourceType.Sound:
						fileExtension = "声音资源 (*.wav, *.mp3)|*.wav;*.mp3";
						resourceName = Path.Combine(AssetCompilerExtended.SourceContent, SoundSign);
						break;
					case ResourceType.Particle:
						fileExtension = "粒子资源 (*.em, *.xml)|*.em;*.xml";
						resourceName = Path.Combine(AssetCompilerExtended.SourceContent, ParticleSign);
						break;
					case ResourceType.Font:
						fileExtension = "字体资源 (*.spritefont, *.fnt, *.png)|*.spritefont;*.fnt;*.png";
						resourceName = Path.Combine(AssetCompilerExtended.SourceContent, FontSign);
						break;
					default:
						fileExtension = "所有资源 (*.*)|*.*";
						resourceName = AssetCompilerExtended.SourceContent;
						break;
				}
			}


			openFileDialog.Filter = fileExtension;
			if (openFileDialog.ShowDialog() == true)
			{
				var assetName = resourceName.InitAsset(openFileDialog.FileName);
				assetName.Compiler(((ResourceModel) ResourceTreeView.SelectedItem).Type);

				InitTreeViewResourceModel(resourceName);
			}
		}

		private void InitTreeViewResourceModel(string resourceName)
		{
			var sourceFileCollection = AssetCompilerExtended.GetMgContent();
			var resourceInfo = GetSignResult(sourceFileCollection.SourceFiles, resourceName);
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

		private List<ResourceInfo> GetSignResult(IEnumerable<string> sourceFiles, string resourceName)
		{
			var files = new List<ResourceInfo>();
			foreach (var sourceFile in sourceFiles)
			{
				if (sourceFile.Replace(@"/", "\\").Contains(resourceName.Replace(@"/", "\\")))
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
