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

    public class LineEmitterTypeConverter : EmitterTypeConverter
    {
        /// <summary>
        /// Adds the descriptors.
        /// </summary>
        /// <param name="descriptors">The descriptors.</param>
        protected override void AddDescriptors(List<SmartMemberDescriptor> descriptors)
        {
            base.AddDescriptors(descriptors);

            var type = typeof(LineEmitter);

            descriptors.AddRange(new SmartMemberDescriptor[]
            {
                new SmartMemberDescriptor(type.GetProperty("Length"),
                    new CategoryAttribute("线发射器"),
                    new DescriptionAttribute("线的长度（以像素为单位）.")),

                new SmartMemberDescriptor(type.GetProperty("Angle"),
                    new CategoryAttribute("线发射器"),
                    new EditorAttribute(typeof(AngleEditor), typeof(UITypeEditor)),
                    new DescriptionAttribute("以弧度表示的线的角度.")),

                new SmartMemberDescriptor(type.GetField("Rectilinear"),
                    new CategoryAttribute("线发射器"),
                    new DescriptionAttribute("如果发射器释放垂直于线的角度的粒子，则为真.")),

                new SmartMemberDescriptor(type.GetField("EmitBothWays"),
                    new CategoryAttribute("线发射器"),
                    new DescriptionAttribute("如果发射器双向释放粒子，则为真。 只有在启用Rectilinear后才能工作.")),

            });
        }
    }
}