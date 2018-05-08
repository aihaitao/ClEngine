using System.Collections;
using System.Windows.Controls;

namespace ClEngine.Particle.TypeEditor
{
	/// <summary>
	/// EmitterEditor.xaml 的交互逻辑
	/// </summary>
	public partial class EmitterEditor : UserControl
	{
		public EmitterEditor(IEnumerable type)
		{
			InitializeComponent();

			MainTreeView.ItemsSource = type;
		}
	}
}
