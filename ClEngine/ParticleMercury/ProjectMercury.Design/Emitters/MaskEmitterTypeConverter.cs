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
    using ProjectMercury.Design.UITypeEditors;
    using ProjectMercury.Emitters;

    public class MaskEmitterTypeConverter : EmitterTypeConverter
    {
        /// <summary>
        /// Adds the descriptors.
        /// </summary>
        /// <param name="descriptors">The descriptors.</param>
        protected override void AddDescriptors(List<SmartMemberDescriptor> descriptors)
        {
            base.AddDescriptors(descriptors);

            var type = typeof(MaskEmitter);

            descriptors.AddRange(new SmartMemberDescriptor[]
            {
                new SmartMemberDescriptor(type.GetProperty("Mask"),
                    new EditorAttribute(typeof(MaskEditor), typeof(UITypeEditor)),
                    new CategoryAttribute("掩码发射器"),
                    new DescriptionAttribute("表示掩码的字节数组.")),

                new SmartMemberDescriptor(type.GetProperty("Threshold"),
                    new CategoryAttribute("掩码发射器"),
                    new DescriptionAttribute("超过这个阈值的粒子将从掩码内的一个点释放.")),

                new SmartMemberDescriptor(type.GetProperty("MaskTextureContentPath"),
                    new CategoryAttribute("掩码发射器"),
                    new DescriptionAttribute("内容项目中用于表示蒙版的纹理的路径.")),

                new SmartMemberDescriptor(type.GetProperty("Width"),
                    new CategoryAttribute("掩码发射器"),
                    new DescriptionAttribute("掩码发射器的宽度.")),

                new SmartMemberDescriptor(type.GetProperty("Height"),
                    new CategoryAttribute("掩码发射器"),
                    new DescriptionAttribute("掩码发射器的高度.")),
            });
        }
    }
}