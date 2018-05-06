using System.Windows.Controls;
using MonoGame.Extended.Particles;

namespace ClEngine.CoreLibrary.Particle
{
	public class EmitterTreeNode : TreeViewItem
	{
		public EmitterTreeNode(ParticleEmitter emitter) : base()
		{
			Guard.ArgumentNull("emitter", emitter);

			Emitter = emitter;

			Header = emitter.Name;

			Tag = emitter;

			foreach (var emitterModifier in emitter.Modifiers)
			{
				var node = new ModifierTreeNode(emitterModifier);

				Items.Add(node);
			}

			IsExpanded = true;
		}

		public ParticleEmitter Emitter { get; private set; }
	}
}