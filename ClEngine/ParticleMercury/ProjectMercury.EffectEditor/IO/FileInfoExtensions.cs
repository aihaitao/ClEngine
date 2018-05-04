/*  
 Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor.IO
{
    using System;
    using System.Globalization;
    using System.IO;
    using Trinet.Core.IO.Ntfs;

    /// <summary>
    /// Provides extension methods to the <see cref="System.IO.FileInfo"/> class.
    /// </summary>
    internal static class FileInfoExtensions
    {
        private const string ZoneIdentifierStreamName = "Zone.Identifier";

        /// <summary>
        /// Unblocks the file.
        /// </summary>
        /// <param name="file">Extension instance.</param>
        public static void Unblock(this FileInfo file)
        {
            if (file.Exists == false)
                throw new FileNotFoundException(String.Format(CultureInfo.InvariantCulture, "指定的文件 '{0}' 无法被找到.", file.FullName));

            if (file.AlternateDataStreamExists(ZoneIdentifierStreamName))
                file.DeleteAlternateDataStream(ZoneIdentifierStreamName);
        }
    }
}