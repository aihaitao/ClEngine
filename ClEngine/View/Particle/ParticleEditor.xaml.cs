using System.Windows;

namespace ClEngine.View.Particle
{
	/// <summary>
	/// ParticleEditor.xaml 的交互逻辑
	/// </summary>
	public partial class ParticleEditor
	{
		public static ParticleEditor Instance { get; private set; }

		public ParticleEditor()
		{
			Instance = this;

			InitializeComponent();
		}

		private void EffectTree_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			ParticlePropertyGrid.SelectedObject = e.NewValue;
		}

		public void SelectObject(object obj)
		{
			ParticlePropertyGrid.SelectedObject = obj;
		}

		public void UpdateData()
		{
			ParticlePropertyGrid.Refresh();
		}
	}
}
