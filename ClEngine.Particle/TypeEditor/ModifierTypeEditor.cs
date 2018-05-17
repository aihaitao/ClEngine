using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;

namespace ClEngine.Particle.TypeEditor
{
	public class ModifierTypeEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			var emitterEditor = new EmitterEditor((IEnumerable)value);
			emitterEditor.ShowDialog();

			return value;
		}
	}
}