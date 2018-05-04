﻿/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor
{
    using System;
    using PluginInterfaces;

    internal class SerializeEventArgs : CoreOperationEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializeEventArgs"/> class.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <param name="filePath">The file path.</param>
        public SerializeEventArgs(IEffectSerializationPlugin plugin, String filePath)
            : base()
        {
            Plugin = plugin;
            FilePath = filePath;
        }

        public IEffectSerializationPlugin Plugin { get; private set; }

        public String FilePath { get; private set; }
    }
}