using System.Windows;

namespace ClEngine.Particle.TypeEditor
{
	/// <summary>
	/// ModifierEditor.xaml 的交互逻辑
	/// </summary>
	public partial class ModifierEditor : Window
	{
		public ModifierEditor()
		{
			InitializeComponent();
		}

		private void MainTreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			EmitterPropertyGrid.SelectedObject = e.NewValue;
		}
	}
}
