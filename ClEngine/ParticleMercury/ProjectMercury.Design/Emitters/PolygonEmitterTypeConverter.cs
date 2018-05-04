/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Design.Emitters
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing.Design;
    using ProjectMercury.Emitters;
    using ProjectMercury.Design.UITypeEditors;

    public class PolygonEmitterTypeConverter : EmitterTypeConverter
    {
        /// <summary>
        /// Adds the descriptors.
        /// </summary>
        /// <param name="descriptors">The descriptors.</param>
        protected override void AddDescriptors(List<SmartMemberDescriptor> descriptors)
        {
            base.AddDescriptors(descriptors);

            var type = typeof(PolygonEmitter);

            descriptors.AddRange(new SmartMemberDescriptor[]
            {
                new SmartMemberDescriptor(type.GetField("Close"),
                    new CategoryAttribute("多边形"),
                    new DescriptionAttribute("如果poly封闭，则为真.")),

                new SmartMemberDescriptor(type.GetProperty("Points"),
                    new CategoryAttribute("多边形"),
                    new EditorAttribute(typeof(PolygonPointCollectionEditor), typeof(UITypeEditor)),
                    new DescriptionAttribute("描述poly的点.")),

                new SmartMemberDescriptor(type.GetProperty("Rotation"),
                    new EditorAttribute(typeof(AngleEditor), typeof(UITypeEditor)),
                    new CategoryAttribute("多边形"),
                    new DescriptionAttribute("以弧度旋转多边形。")),

                new SmartMemberDescriptor(type.GetProperty("Scale"),
                    new CategoryAttribute("多边形"),
                    new DescriptionAttribute("缩放比例")),

                new SmartMemberDescriptor(type.GetProperty("Origin"),
                    new CategoryAttribute("多边形"),
                    new DescriptionAttribute("多边形的源头."))
            });
        }
    }
}