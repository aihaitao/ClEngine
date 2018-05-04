/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Design.Emitters
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing.Design;
    using ProjectMercury.Design.Modifiers;
    using ProjectMercury.Design.UITypeEditors;
    using ProjectMercury.Emitters;

    public class EmitterTypeConverter : ExpandableObjectConverter
    {
        /// <summary>
        /// Gets a value indicating whether this object supports properties using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
        /// <returns>
        /// true because <see cref="M:System.ComponentModel.TypeConverter.GetProperties(System.Object)"/> should be called to find the properties of this object. This method never returns false.
        /// </returns>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Adds the descriptors.
        /// </summary>
        /// <param name="descriptors">The descriptors.</param>
        protected virtual void AddDescriptors(List<SmartMemberDescriptor> descriptors)
        {
            var type = typeof(Emitter);

            descriptors.AddRange(new SmartMemberDescriptor[]
            {
                new SmartMemberDescriptor(type.GetProperty("Name"),
                    new CategoryAttribute("发射器"),
                    new DescriptionAttribute("发射器的名称.")),

                new SmartMemberDescriptor(type.GetField("Enabled"),
                    new CategoryAttribute("发射器"),
                    new DisplayNameAttribute("是否启用"),
                    new DescriptionAttribute("启用或禁用触发发射器.")),

                new SmartMemberDescriptor(type.GetField("BlendMode"),
                    new CategoryAttribute("发射器"),
                    new DisplayNameAttribute("混合模式"),
                    new DescriptionAttribute("渲染发射器时使用的混合模式.")),

                new SmartMemberDescriptor(type.GetField("ParticleTextureAssetName"),
                    new CategoryAttribute("发射器"),
                    new DisplayNameAttribute("纹理资产名称"),
                    new DescriptionAttribute("项目中纹理资源的名称")),

                new SmartMemberDescriptor(type.GetField("TriggerOffset"),
                    new CategoryAttribute("发射器"),
                    new DisplayNameAttribute("触发偏移"),
                    new DescriptionAttribute("发射器触发与ParticleEffect相关的偏移量.")),

                new SmartMemberDescriptor(type.GetField("MinimumTriggerPeriod"),
                    new CategoryAttribute("发射器"),
                    new DisplayNameAttribute("最小触发周期"),
                    new DescriptionAttribute("发射器触发器之间的最短时间（全部和小数秒).")),

                new SmartMemberDescriptor(type.GetField("ReleaseColour"),
                    new CategoryAttribute("发射器"),
                    new EditorAttribute(typeof(VariableColourEditor), typeof(UITypeEditor)),
                    new DisplayNameAttribute("初始颜色"),
                    new DescriptionAttribute("粒子的初始颜色，因为它们由发射器释放.")),

                new SmartMemberDescriptor(type.GetField("ReleaseImpulse"),
                    new CategoryAttribute("发射器"),
                    new DisplayNameAttribute("Release Impulse"),
                    new DescriptionAttribute("最初的冲动适用于粒子，因为它们被释放.")),

                new SmartMemberDescriptor(type.GetField("ReleaseOpacity"),
                    new CategoryAttribute("发射器"),
                    new DisplayNameAttribute("初始透明度"),
                    new DescriptionAttribute("粒子的初始不透明度，因为它们由发射器释放.")),

                new SmartMemberDescriptor(type.GetProperty("ReleaseQuantity"),
                    new CategoryAttribute("发射器"),
                    new DisplayNameAttribute("初始数量"),
                    new DescriptionAttribute("发射器在每个触发器上释放的粒子数量.")),

                new SmartMemberDescriptor(type.GetField("ReleaseRotation"),
                    new CategoryAttribute("发射器"),
                    //new EditorAttribute(typeof(AngleEditor), typeof(UITypeEditor)),
                    new DisplayNameAttribute("Release Rotation"),
                    new DescriptionAttribute("粒子的初始旋转（以弧度表示），因为它们由发射器释放.")),

                new SmartMemberDescriptor(type.GetField("ReleaseSpeed"),
                    new CategoryAttribute("发射器"),
                    new DisplayNameAttribute("初始速度"),
                    new DescriptionAttribute("粒子在发射器释放时的初始速度.")),
                
                new SmartMemberDescriptor(type.GetField("ReleaseScale"),
                    new CategoryAttribute("发射器"),
                    new DisplayNameAttribute("初始比例"),
                    new DescriptionAttribute("粒子的初始比例，因为它们是由发射器发射的.")),
            });
        }

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
            List<SmartMemberDescriptor> descriptors = new List<SmartMemberDescriptor>();

            this.AddDescriptors(descriptors);

            return new PropertyDescriptorCollection((PropertyDescriptor[])descriptors.ToArray());
        }
    }
}