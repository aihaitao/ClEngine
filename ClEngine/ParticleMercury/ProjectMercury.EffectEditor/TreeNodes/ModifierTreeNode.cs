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
    using Modifiers;

    public class ModifierTreeNode : TreeNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModifierTreeNode"/> class.
        /// </summary>
        /// <param name="modifier">The modifier.</param>
        public ModifierTreeNode(Modifier modifier)
            : base()
        {
	        Modifier = modifier ?? throw new ArgumentNullException(nameof(modifier));

            Text = modifier.GetType().Name;

            Tag = modifier;
        }

        /// <summary>
        /// Gets or sets the modifier.
        /// </summary>
        /// <value>The modifier.</value>
        public Modifier Modifier { get; private set; }
    }
}