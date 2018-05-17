using System.Windows;
using ClEngine.CoreLibrary.Asset;
using Microsoft.Win32;

namespace ClEngine.View.Asset
{
	public partial class ResourceView
	{
		private void AddResourceOnClick(object sender, RoutedEventArgs e)
		{
			var openFileDialog = new OpenFileDialog
			{
				Multiselect = false,
				CheckFileExists = true,
				CheckPathExists = true,
			};

			if (ResourceTreeView.SelectedItem == null)
			{
				Log.Error("清先选择要加入的资源类型");

				return;
			}

			if (ResourceTreeView.SelectedItem is AssetResolver resolver)
			{
				openFileDialog.Filter = resolver.Extension;
				if (openFileDialog.ShowDialog() == true)
				{
					resolver.Compiler(openFileDialog.FileName);
				}
			}
		}

		private void ResourceTreeViewOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (e.NewValue is AssetResolver resolver)
			{
				var watcher = AssetHelper.GetFileSystemWatcher(resolver.StoragePath);
				watcher.Filter = resolver.Extension;
			}
		}
	}
}