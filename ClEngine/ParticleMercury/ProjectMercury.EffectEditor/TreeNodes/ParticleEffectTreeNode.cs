/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor.TreeNodes
{
    using System;
    using System.Windows.Forms;
    using Emitters;

    public class ParticleEffectTreeNode : TreeNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffectTreeNode"/> class.
        /// </summary>
        /// <param name="effect">The effect.</param>
        public ParticleEffectTreeNode(ParticleEffect effect)
            : base()
        {
	        ParticleEffect = effect ?? throw new ArgumentNullException(nameof(effect));

            Text = effect.Name ?? "粒子效果";

            ParticleEffect.NameChanged += (o, e) => Text = ParticleEffect.Name;

            Tag = effect;

            foreach (Emitter emitter in effect)
            {
                EmitterTreeNode node = new EmitterTreeNode(emitter);

                Nodes.Add(node);
            }
        }

        /// <summary>
        /// Gets or sets the particle effect.
        /// </summary>
        /// <value>The particle effect.</value>
        public ParticleEffect ParticleEffect { get; private set; }
    }
}