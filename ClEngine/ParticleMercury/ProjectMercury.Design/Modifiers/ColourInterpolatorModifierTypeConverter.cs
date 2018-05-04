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

    public class ColourInterpolatorModifierTypeConverter : ExpandableObjectConverter
    {
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            var type = typeof(ColourInterpolatorModifier);

            return new PropertyDescriptorCollection(new PropertyDescriptor[]
            {
                new SmartMemberDescriptor(type.GetProperty("InitialColour"),
                    new DisplayNameAttribute("初始颜色"),
                    new EditorAttribute(typeof(VectorColourEditor), typeof(UITypeEditor)),
                    new DescriptionAttribute("粒子从发射器释放时的初始颜色。")),

                new SmartMemberDescriptor(type.GetProperty("MiddleColour"),
                    new DisplayNameAttribute("中间颜色"),
                    new EditorAttribute(typeof(VectorColourEditor), typeof(UITypeEditor)),
                    new DescriptionAttribute("粒子的中间颜色，达到了确定的中间位置.")),

                new SmartMemberDescriptor(type.GetProperty("MiddlePosition"),
                    new DisplayNameAttribute("中间位置"),
                    new EditorAttribute(typeof(PercentEditor), typeof(UITypeEditor)),
                    new DescriptionAttribute("粒子中的点达到中等颜色.")),

                new SmartMemberDescriptor(type.GetProperty("FinalColour"),
                    new DisplayNameAttribute("最终颜色"),
                    new EditorAttribute(typeof(VectorColourEditor), typeof(UITypeEditor)),
                    new DescriptionAttribute("粒子结束后的最终颜色."))
            });
        }
    }
}