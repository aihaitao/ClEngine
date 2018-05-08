using System.Collections.Generic;
using System.Windows.Forms;

namespace ClEngine.Particle.TypeEditor
{
	public partial class EmitterHostEditor : Form
	{
		public EmitterHostEditor()
		{
			InitializeComponent();

			elementHost.Child = new EmitterEditor(new List<string>());
		}
	}
}
