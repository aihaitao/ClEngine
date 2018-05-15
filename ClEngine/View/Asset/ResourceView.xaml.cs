using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using ClEngine.CoreLibrary.Asset;
using ClEngine.Model;
using ClEngine.ViewModel;

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
			ResourceDataGrid.SelectionChanged += ResourceDataGridOnSelectionChanged;

			CreateContextMenu();
		}

		private void CreateContextMenu()
		{
			var contextMenu = new ContextMenu();

			var resourceReName = new MenuItem { Header = "ReName".GetTranslateName() };
			var addResource = new MenuItem { Header = "AddResource".GetTranslateName() };
			var removeResource = new MenuItem { Header = "RemoveResource".GetTranslateName() };
			var separator = new Separator();
			var copyName = new MenuItem { Header = "CopyName".GetTranslateName() };

			contextMenu.Items.Add(resourceReName);
			contextMenu.Items.Add(addResource);
			contextMenu.Items.Add(removeResource);
			contextMenu.Items.Add(separator);
			contextMenu.Items.Add(copyName);

			addResource.Click += AddResourceOnClick;

			ResourceDataGrid.ContextMenu = contextMenu;
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
	}
}
