/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Design.Modifiers
{
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using ProjectMercury.Design.UITypeEditors;
    using ProjectMercury.Modifiers;

    public class OpacityInterpolatorModifierTypeConverter : ExpandableObjectConverter
    {
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            var type = typeof(OpacityInterpolatorModifier);

            return new PropertyDescriptorCollection(new PropertyDescriptor[]
            {
                new SmartMemberDescriptor(type.GetProperty("InitialOpacity"),
                    new DisplayNameAttribute("初始透明度"),
                    new EditorAttribute(typeof(PercentEditor), typeof(UITypeEditor)),
                    new DescriptionAttribute("粒子从发射器释放时的初始透明度.")),

                new SmartMemberDescriptor(type.GetProperty("MiddleOpacity"),
                    new DisplayNameAttribute("中间透明度"),
                    new EditorAttribute(typeof(PercentEditor), typeof(UITypeEditor)),
                    new DescriptionAttribute("粒子的透明度，因为它们达到了确定的中间位置.")),

                new SmartMemberDescriptor(type.GetProperty("MiddlePosition"),
                    new DisplayNameAttribute("中间位置"),
                    new EditorAttribute(typeof(PercentEditor), typeof(UITypeEditor)),
                    new DescriptionAttribute("粒子达到中等透明度的一点.")),

                new SmartMemberDescriptor(type.GetProperty("FinalOpacity"),
                    new DisplayNameAttribute("最终透明度"),
                    new EditorAttribute(typeof(PercentEditor), typeof(UITypeEditor)),
                    new DescriptionAttribute("粒子结束后的最终透明度."))
            });
        }
    }
}