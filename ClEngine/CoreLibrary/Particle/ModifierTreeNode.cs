using System;
using System.Windows.Controls;
using MonoGame.Extended.Particles.Modifiers;

namespace ClEngine.CoreLibrary.Particle
{
	public class ModifierTreeNode : TreeViewItem
	{
		public ModifierTreeNode(Modifier modifier)
			: base()
		{
			Modifier = modifier ?? throw new ArgumentNullException(nameof(modifier));

			Header = modifier.GetType().Name;

			Tag = modifier;
		}

		public Modifier Modifier { get; private set; }
	}
}