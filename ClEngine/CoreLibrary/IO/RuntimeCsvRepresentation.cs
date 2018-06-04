using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ClEngine.CoreLibrary.Instructions.Reflection;
using ClEngine.CoreLibrary.IO.Csv;
using ClEngine.CoreLibrary.Utilities;
// ReSharper disable RedundantAssignment

namespace ClEngine.CoreLibrary.IO
{
    public struct CsvHeader : IEquatable<CsvHeader>
    {
        public string Name;
        public MemberTypes MemberTypes;
        public bool IsRequired;
        public string OriginalText;

        public CsvHeader(string name)
        {
            Name = name;
            OriginalText = name;
            IsRequired = false;
            MemberTypes = 0;
        }

        public bool Equals(CsvHeader other)
        {
            return Name == other.Name &&
                   IsRequired == other.IsRequired &&
                   MemberTypes == other.MemberTypes;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static string GetNameWithoutParentheses(string text)
        {
            var nameWithoutParentheses = StringFunctions.RemoveWhitespace(text);

            if (!nameWithoutParentheses.Contains("(")) return nameWithoutParentheses;
            var openingIndex = nameWithoutParentheses.IndexOf('(');


            nameWithoutParentheses = nameWithoutParentheses.Substring(0, openingIndex);
            return nameWithoutParentheses;
        }
    }

    public class RuntimeCsvRepresentation
    {
        public CsvHeader[] Headers;
        public List<string[]> Records;

        public void CreateObjectList(Type typeOfElement, IList listToPopulate, string contentManagerName)
        {
#if WINDOWS_8 || UWP
            bool isPrimitive = typeOfElement.IsPrimitive();
#else
            bool isPrimitive = typeOfElement.IsPrimitive;
#endif
            if (isPrimitive || typeOfElement == typeof(string))
            {
                if (typeOfElement == typeof(string))
                {
                    listToPopulate.Add(Headers[0].OriginalText);

                    foreach (var t in Records)
                    {
                        listToPopulate.Add(t[0]);
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            else if (typeOfElement == typeof(List<string>))
            {
                foreach (var record in Records)
                {
                    var row = new List<string>();
                    listToPopulate.Add(row);
                    row.AddRange(record);
                }
            }
            else if (typeOfElement == typeof(string[]))
            {
                foreach (var t in Records)
                {
                    listToPopulate.Add(t);
                }
            }
            
            else
            {
                CreateNonPrimitiveList(typeOfElement, listToPopulate, contentManagerName);
            }


        }

        private void CreateNonPrimitiveList(Type typeOfElement, IList listToPopulate, string contentManagerName)
        {
            GetReflectionInformation(typeOfElement, out var memberTypeIndexPairs, out var propertyInfosEnumerable,
                out var fieldInfosEnumerable);

            var propertyInfos = new List<PropertyInfo>(propertyInfosEnumerable);
            var fieldInfos = new List<FieldInfo>(fieldInfosEnumerable);

            var numberOfColumns = Headers.Length;

            object lastElement = null;
            var wasRequiredMissing = false;


            for (var row = 0; row < Records.Count; row++)
            {
                wasRequiredMissing = TryCreateNewObjectFromRow(
                    typeOfElement,
                    contentManagerName,
                    memberTypeIndexPairs,
                    propertyInfos,
                    fieldInfos,
                    numberOfColumns,
                    lastElement,
                    wasRequiredMissing,
                    row,
                    out var newElement,
                    out var newElementFailed);

                if (newElementFailed || wasRequiredMissing) continue;
                listToPopulate.Add(newElement);

                lastElement = newElement;
            }
        }


        private bool TryCreateNewObjectFromRow(Type typeOfElement, string contentManagerName,
            MemberTypeIndexPair[] memberTypeIndexPairs,
            List<PropertyInfo> propertyInfos, List<FieldInfo> fieldInfos, int numberOfColumns, object lastElement, int row, out object newElement, out bool newElementFailed)
        {
            return TryCreateNewObjectFromRow(typeOfElement, contentManagerName, memberTypeIndexPairs, propertyInfos, fieldInfos, numberOfColumns, lastElement, false, row, out newElement, out newElementFailed);
        }

        private bool TryCreateNewObjectFromRow(Type typeOfElement, string contentManagerName,
            MemberTypeIndexPair[] memberTypeIndexPairs,
            List<PropertyInfo> propertyInfos, List<FieldInfo> fieldInfos, int numberOfColumns, object lastElement,
            bool wasRequiredMissing, int row, out object newElement, out bool newElementFailed)
        {
            wasRequiredMissing = false;
            newElementFailed = false;

            if (typeOfElement == typeof(string[]))
            {
                var requiredColumn = -1;

                for (var i = 0; i < Headers.Length; i++)
                {
                    if (!Headers[i].IsRequired) continue;
                    requiredColumn = i;
                    break;
                }

                if (requiredColumn != -1 && string.IsNullOrEmpty(Records[row][requiredColumn]))
                {
                    wasRequiredMissing = true;
                    newElement = null;
                }
                else
                {
                    var returnObject = new string[numberOfColumns];

                    for (var column = 0; column < numberOfColumns; column++)
                    {
                        returnObject[column] = Records[row][column];
                    }

                    newElement = returnObject;
                }
            }

            else
            {

                var isComment = Records[row] != null && Records[row][0].StartsWith("//");

                if (isComment)
                {
                    wasRequiredMissing = true;
                    newElementFailed = true;
                    newElement = null;
                }
                else
                {

                    newElement = Activator.CreateInstance(typeOfElement);
                    wasRequiredMissing = AsssignValuesOnElement(
                        contentManagerName,
                        memberTypeIndexPairs,
                        propertyInfos,
                        fieldInfos,
                        numberOfColumns,
                        lastElement,
                        false,
                        row,
                        newElement);
                }
            }

            return wasRequiredMissing;
        }

        private bool AsssignValuesOnElement(string contentManagerName, MemberTypeIndexPair[] memberTypeIndexPairs, List<PropertyInfo> propertyInfos, List<FieldInfo> fieldInfos, int numberOfColumns, object lastElement, bool wasRequiredMissing, int row, object newElement)
        {
            for (var column = 0; column < numberOfColumns; column++)
            {
                if (memberTypeIndexPairs[column].Index == -1) continue;
                var objectToSetValueOn = newElement;
                if (wasRequiredMissing)
                {
                    objectToSetValueOn = lastElement;
                }
                var columnIndex = memberTypeIndexPairs[column].Index;

                var isRequired = Headers[column].IsRequired;

                if (isRequired && string.IsNullOrEmpty(Records[row][column]))
                {
                    wasRequiredMissing = true;
                    continue;
                }

                #region If the member is a Property, so set the value obtained from converting the string.
                if (memberTypeIndexPairs[column].MemberType == MemberTypes.Property)
                {
                    var propertyInfo = propertyInfos[memberTypeIndexPairs[column].Index];

                    var propertyType = propertyInfo.PropertyType;

                    var isList = propertyInfo.PropertyType.Name == "List`1";
                    if (isList)
                    {
                        propertyType = propertyType.GetGenericArguments()[0];
                    }

                    object valueToSet;

                    var cellValue = Records[row][column];
                    if (string.IsNullOrEmpty(cellValue))
                    {
                        valueToSet = null;
                    }
                    else
                    {
                        try
                        {


                            valueToSet = PropertyValuePair.ConvertStringToType(
                                Records[row][column],
                                propertyType,
                                contentManagerName);
                        }
                        catch (ArgumentException e)
                        {
                            throw new Exception("Could not set variable " + propertyInfo.Name + " to " + cellValue + "\n\n" + e, e);
                        }
                        catch (FormatException)
                        {
                            throw new Exception("Error parsing the value " + cellValue + " for the property " + propertyInfo.Name);
                        }
                    }

                    if (isList)
                    {
                        // todo - need to support adding to property lists
                        var objectToCallOn = propertyInfo.GetValue(objectToSetValueOn, null);
                        if (objectToCallOn == null)
                        {
                            objectToCallOn = Activator.CreateInstance(propertyInfo.PropertyType);

                            propertyInfo.SetValue(objectToSetValueOn, objectToCallOn, null);
                        }

                        if (valueToSet == null) continue;
                        var methodInfo = propertyInfo.PropertyType.GetMethod("Add");

                        if (methodInfo != null) methodInfo.Invoke(objectToCallOn, new[] {valueToSet});
                    }
                    else if (!wasRequiredMissing)
                    {
                        propertyInfo.SetValue(
                            objectToSetValueOn,
                            valueToSet,
                            null);
                    }

                }
                #endregion
                
                else if (memberTypeIndexPairs[column].MemberType == MemberTypes.Field)
                {
                    {
                        GetFieldValueToSet(contentManagerName, fieldInfos, row, column, columnIndex, out var fieldInfo,
                            out var isList, out var valueToSet);

                        if (isList)
                        {
                            object objectToCallOn = fieldInfo.GetValue(objectToSetValueOn);
                            if (objectToCallOn == null)
                            {
                                objectToCallOn = Activator.CreateInstance(fieldInfo.FieldType);

                                fieldInfo.SetValue(objectToSetValueOn, objectToCallOn);
                            }

                            if (valueToSet != null)
                            {
                                MethodInfo methodInfo = fieldInfo.FieldType.GetMethod("Add");

                                if (methodInfo != null)
                                    methodInfo.Invoke(objectToCallOn, new[] {valueToSet});
                            }
                        }
                        else if (!wasRequiredMissing)
                        {
                            fieldInfo.SetValue(objectToSetValueOn, valueToSet);
                        }
                    }
                }
            }

            return wasRequiredMissing;
        }

        private void GetFieldValueToSet(string contentManagerName, IReadOnlyList<FieldInfo> fieldInfos, int row, int column, int columnIndex, out FieldInfo fieldInfo, out bool isList, out object valueToSet)
        {
            fieldInfo = fieldInfos[columnIndex];

            var type = fieldInfo.FieldType;
            var name = fieldInfo.Name;


            isList = type.Name == "List`1";

            var typeOfObjectInCell = type;

            if (isList)
            {
                typeOfObjectInCell = type.GetGenericArguments()[0];
            }

            var cellValue = Records[row][column];
            if (string.IsNullOrEmpty(cellValue))
            {
                valueToSet = null;
            }
            else
            {
                try
                {
                    valueToSet = PropertyValuePair.ConvertStringToType(
                        Records[row][column],
                        typeOfObjectInCell,
                        contentManagerName);
                }
                catch (ArgumentException e)
                {
                    throw new Exception("Could not set variable " + name + " to " + cellValue + "\n\n" + e, e);
                }
                catch (FormatException)
                {
                    throw new Exception("Error parsing the value " + cellValue + " for the property " + name);
                }
            }
        }

        private void GetReflectionInformation(Type typeOfElement, out MemberTypeIndexPair[] memberTypeIndexPairs, out IEnumerable<PropertyInfo> propertyInfos, out IEnumerable<FieldInfo> fieldInfos)
        {
            memberTypeIndexPairs = new MemberTypeIndexPair[Headers.Length];

            propertyInfos = typeOfElement.GetProperties();
            fieldInfos = typeOfElement.GetFields();

            RemoveHeaderWhitespaceAndDetermineIfRequired();


            BuildMemberTypeIndexInformation(memberTypeIndexPairs, propertyInfos, fieldInfos);
        }

        private void BuildMemberTypeIndexInformation(MemberTypeIndexPair[] memberTypeIndexPairs,
            IEnumerable propertyInfos, IEnumerable fieldInfos)
        {
            var enumerable = fieldInfos as object[] ?? fieldInfos.Cast<object>().ToArray();
            var infos = propertyInfos as object[] ?? propertyInfos.Cast<object>().ToArray();
            for (var i = 0; i < Headers.Length; i++)
            {
                memberTypeIndexPairs[i].Index = -1;

                var j = 0;
                foreach (FieldInfo fieldInfo in enumerable)
                {
                    if (fieldInfo.Name == Headers[i].Name)
                    {
                        Headers[i].MemberTypes = MemberTypes.Field;

                        memberTypeIndexPairs[i] = new MemberTypeIndexPair
                        {
                            Index = j,
                            MemberType = MemberTypes.Field
                        };
                        break;
                    }

                    j++;
                }

                if (memberTypeIndexPairs[i].Index != -1 && memberTypeIndexPairs[i].MemberType == MemberTypes.Field)
                {
                    continue;
                }

                j = 0;
                foreach (PropertyInfo propertyInfo in infos)
                {
                    if (propertyInfo.Name == Headers[i].Name)
                    {
                        Headers[i].MemberTypes = MemberTypes.Property;

                        memberTypeIndexPairs[i] = new MemberTypeIndexPair
                        {
                            Index = j,
                            MemberType = MemberTypes.Property
                        };
                        break;
                    }

                    j++;
                }

                if (memberTypeIndexPairs[i].Index != -1 && memberTypeIndexPairs[i].MemberType == MemberTypes.Property)
                {
                    continue;
                }

                memberTypeIndexPairs[i].Index = -1;
            }
        }

        public void RemoveHeaderWhitespaceAndDetermineIfRequired()
        {
            for (var i = 0; i < Headers.Length; i++)
            {
                var text = Headers[i].OriginalText;
                var isRequired = IsHeaderRequired(text);

                var nameWithoutParentheses = CsvHeader.GetNameWithoutParentheses(text);



                Headers[i].Name = nameWithoutParentheses;
                Headers[i].IsRequired = isRequired;
            }
        }

        private static bool IsHeaderRequired(string header)
        {
            var isRequired = false;
            var openingIndex = header.IndexOf('(');
            if (openingIndex == -1)
            {
            }
            else
            {
                var qualifiers = header.Substring(openingIndex + 1, header.Length - (openingIndex + 1) - 1);

                if (qualifiers.Contains(","))
                {
                    var brokenUp = qualifiers.Split(',');

                    foreach (var s in brokenUp)
                    {
                        if (s.Trim() == "required")
                        {
                            isRequired = true;
                        }
                    }
                }
                else
                {
                    if (qualifiers == "required")
                    {
                        isRequired = true;
                    }
                }
            }
            return isRequired;

        }
    }


}