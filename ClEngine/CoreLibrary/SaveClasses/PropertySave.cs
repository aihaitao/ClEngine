﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Serialization;
using FlatRedBall.Math.Geometry;

namespace ClEngine.CoreLibrary.SaveClasses
{
    public class PropertySave
    {
        public string Name;

        [XmlElement("ValueAsString", typeof(string))]
        [XmlElement("ValueAsFloat", typeof(float))]
        [XmlElement("ValueAsInt", typeof(int))]
        [XmlElement("ValueAsBool", typeof(bool))]
        [XmlElement("ValueAsLong", typeof(long))]
        [XmlElement("ValueAsDouble", typeof(double))]
        [XmlElement("ValueAsObject", typeof(object))]
        [XmlElement("ValueAsIntRectangle", typeof(Rectangle))]
        [XmlElement("ValueAsRectangle", typeof(FloatRectangle))]
        // Can't do this because it's an abstract class
        //[XmlElement("ValueAsEnum", typeof(Enum))]
        public object Value;
    }

    public static class PropertySaveListExtensions
    {
        public static bool ContainsValue(this List<PropertySave> propertySaveList, string nameToSearchFor)
        {
            foreach (PropertySave propertySave in propertySaveList)
            {
                if (propertySave.Name == nameToSearchFor)
                {
                    return true;
                }
            }
            return false;
        }

        public static object GetValue(this List<PropertySave> propertySaveList, string nameToSearchFor)
        {
            foreach (PropertySave propertySave in propertySaveList)
            {
                if (propertySave.Name == nameToSearchFor)
                {
                    return propertySave.Value;
                }
            }
            return null;
        }

        public static T GetValue<T>(this List<PropertySave> propertySaveList, string nameToSearchFor)
        {
            foreach (PropertySave propertySave in propertySaveList)
            {
                if (propertySave.Name == nameToSearchFor)
                {
                    return (T)propertySave.Value;
                }
            }
            return default(T);
        }

        public static void Remove(this List<PropertySave> propertySaveList, string nameToRemove)
        {
            for (int i = 0; i < propertySaveList.Count; i++)
            {
                if (propertySaveList[i].Name == nameToRemove)
                {
                    propertySaveList.RemoveAt(i);
                    break;
                }
            }
        }

        public static void SetValue(this List<PropertySave> propertySaveList, string nameToSearchFor, object value)
        {
            bool isDefault = IsValueDefault(value);

            if (isDefault)
            {
                propertySaveList.Remove(nameToSearchFor);
            }
            else
            {
                var existingProperty = propertySaveList.FirstOrDefault(item => item.Name == nameToSearchFor);
                if (existingProperty != null)
                {

                    existingProperty.Value = value;
                }
                else
                {
                    // If we got here then that means there isn't already something in place for this
                    PropertySave newPropertySave = new PropertySave();
                    newPropertySave.Name = nameToSearchFor;
                    newPropertySave.Value = value;
                    propertySaveList.Add(newPropertySave);
                }
            }
        }

        static bool IsValueDefault(object value)
        {
            if (value is bool)
            {
                return ((bool)value) == false;
            }
            if (value is byte)
            {
                return (byte)value == 0;
            }
            if (value is double)
            {
                return (double)value == 0;
            }

            if (value is float)
            {
                return (float)value == 0.0f;
            }
            if (value is int)
            {
                return (int)value == 0;
            }

            if (value is long)
            {
                return (long)value == 0;
            }

            if (value is string)
            {
                return string.IsNullOrEmpty((string)value);
            }

            return false;


        }

    }
}