using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace ClEngine.Particle.TypeEditor
{
	public class EmitterTypeEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			var emitterHostEditor = new EmitterHostEditor();
			emitterHostEditor.ShowDialog();

			return value;
		}
	}
}