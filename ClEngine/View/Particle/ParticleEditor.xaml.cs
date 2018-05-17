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

		public void SelectObject(object obj)
		{
			ParticlePropertyGrid.SelectedObject = obj;
		}
	}
}
