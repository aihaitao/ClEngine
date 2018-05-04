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
    using ProjectMercury.Modifiers;

    public class RadialGravityModifierTypeConverter : ExpandableObjectConverter
    {
        /// <summary>
        /// Gets a collection of properties for the type of object specified by the value parameter.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="value">An <see cref="T:System.Object"/> that specifies the type of object to get the properties for.</param>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute"/> that will be used as a filter.</param>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> with the properties that are exposed for the component, or null if there are no properties.
        /// </returns>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            var type = typeof(RadialGravityModifier);

            return new PropertyDescriptorCollection(new PropertyDescriptor[]
            {
                new SmartMemberDescriptor(type.GetField("Position"),
                    new DescriptionAttribute("屏幕坐标中引力井的triggerPosition.")),

                new SmartMemberDescriptor(type.GetProperty("Radius"),
                    new DescriptionAttribute("重力的半径以像素为单位.")),

                new SmartMemberDescriptor(type.GetProperty("InnerRadius"),
                    new DescriptionAttribute("以像素为单位的重力井的内半径，粒子不会被吸引."),
                    new DisplayNameAttribute("内半径")),

                new SmartMemberDescriptor(type.GetField("Strength"),
                    new DescriptionAttribute("重力的强度以每秒像素为单位."))
            });
        }
    }
}