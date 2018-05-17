using System.Collections;
using System.Windows;

namespace ClEngine.Particle.TypeEditor
{
	/// <summary>
	/// EmitterEditor.xaml 的交互逻辑
	/// </summary>
	public partial class EmitterEditor : Window
	{
		public EmitterEditor(IEnumerable type)
		{
			InitializeComponent();

			MainTreeView.ItemsSource = type;
		}

		private void MainTreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			EmitterPropertyGrid.SelectedObject = e.NewValue;
		}
	}
}
