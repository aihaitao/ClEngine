using System;
using System.Windows.Controls;
using MonoGame.Extended.Particles;

namespace ClEngine.CoreLibrary.Particle
{
	public class ParticleEffectTreeNode : TreeViewItem
	{
		public ParticleEffectTreeNode(ParticleEffect effect)
			: base()
		{
			ParticleEffect = effect ?? throw new ArgumentNullException(nameof(effect));

			Header = effect.Name ?? "粒子效果";

			Tag = effect;

			foreach (var emitter in effect.Emitters)
			{
				var node = new EmitterTreeNode(emitter);

				Items.Add(node);
			}
		}

		public ParticleEffect ParticleEffect { get; private set; }
	}
}