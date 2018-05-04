﻿/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor.DefaultPluginLibrary.ModifierPlugins
{
    using System;
    using System.ComponentModel.Composition;
    using System.Drawing;
    using ProjectMercury.EffectEditor.PluginInterfaces;
    using ProjectMercury.Modifiers;

    [Export(typeof(IModifierPlugin))]
    public class ScaleMergeModifierPlugin : IModifierPlugin
    {
        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        /// <value></value>
        public string Name
        {
            get { return "Scale Merge Modifier"; }
        }

        /// <summary>
        /// Gets the author of the plugin.
        /// </summary>
        /// <value></value>
        public string Author
        {
            get { return "CLEngine"; }
        }

        /// <summary>
        /// Gets the name of the plugin library, if any.
        /// </summary>
        /// <value></value>
        public string Library
        {
            get { return "DefaultPluginLibrary"; }
        }

        /// <summary>
        /// Gets the version number of the plugin.
        /// </summary>
        /// <value></value>
        public Version Version
        {
            get { return new Version(1, 0, 0, 0); }
        }

        /// <summary>
        /// Gets the minimum version of the engine with which the plugin is compatible.
        /// </summary>
        /// <value></value>
        public Version MinimumRequiredVersion
        {
            get { return new Version(3, 1, 0, 0); }
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return "缩放合并修改器"; }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return "一个修改器，它将粒子的尺度在其整个生命周期内合并为一个单一尺度。 当粒子以随机比例释放时效果最好，在这种情况下，要求粒子在使用寿命结束时具有均匀的比例."; }
        }

        /// <summary>
        /// Gets the display icon.
        /// </summary>
        /// <value>The display icon.</value>
        public Icon DisplayIcon
        {
            get { return Icons.Modifier; }
        }

        /// <summary>
        /// Gets the category of the modifier.
        /// </summary>
        /// <value></value>
        public string Category
        {
            get { return "比例"; }
        }


        /// <summary>
        /// Creates a default instance of the Modifier type provided by the plugin.
        /// </summary>
        /// <returns>
        /// An instance of the Modifier type provided by the plugin.
        /// </returns>
        public Modifier CreateDefaultInstance()
        {
            return new ScaleMergeModifier
            {
                MergeScale = 128f
            };
        }
    }
}