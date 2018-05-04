/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Design
{
    using System;
    using System.ComponentModel;

    public class ParticleEffectTypeConverter : ExpandableObjectConverter
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
            var type = typeof(ParticleEffect);

            return new PropertyDescriptorCollection(new PropertyDescriptor[]
            {
                new SmartMemberDescriptor(type.GetProperty("Name"),
	                new DisplayNameAttribute("名称"),
					new DescriptionAttribute("ParticleEffect的名称.")),

                new SmartMemberDescriptor(type.GetField("Author"),
	                new DisplayNameAttribute("作者"),
					new DescriptionAttribute("ParticleEffect的作者.")),

                new SmartMemberDescriptor(type.GetField("Description"),
	                new DisplayNameAttribute("描述"),
					new DescriptionAttribute("粒子效应的描述."))
            });
        }
    }
}