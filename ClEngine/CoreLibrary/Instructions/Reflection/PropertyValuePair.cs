using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using ClEngine.CoreLibrary.Utilities;
using ClEngine.Properties;
using FlatRedBall;
using FlatRedBall.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClEngine.CoreLibrary.Instructions.Reflection
{
    public struct PropertyValuePair
    {
        internal static Dictionary<string, Type> MUnqualifiedTypeDictionary = new Dictionary<string, Type>();

        public static List<Assembly> AdditionalAssemblies
        {
            get;
            private set;
        }

        public static T ConvertStringToType<T>(string value)
        {
            return (T)ConvertStringToType(value, typeof(T));
        }

        public static object ConvertStringToType(string value, Type typeToConvertTo)
        {
#if FRB_RAW
            return ConvertStringToType(value, typeToConvertTo, "Global");
#else
            return ConvertStringToType(value, typeToConvertTo, FlatRedBallServices.GlobalContentManager);
#endif
        }

        public static object ConvertStringToType(string value, Type typeToConvertTo, string contentManagerName)
        {
            return ConvertStringToType(value, typeToConvertTo, contentManagerName, false);

        }

        public static object ConvertStringToType(string value, Type typeToConvertTo, string contentManagerName, bool trimQuotes)
        {
            if (IsGenericList(typeToConvertTo))
            {
                return CreateGenericListFrom(value, typeToConvertTo, contentManagerName);
            }
            else
            {
                return ConvertStringValueToValueOfType(value, typeToConvertTo.FullName, contentManagerName, trimQuotes);
            }
        }

        public static object ConvertStringValueToValueOfType(string value, string desiredType, string contentManagerName, bool trimQuotes)
        {
            value = value.Trim();

            if (trimQuotes ||
                desiredType != typeof(string).FullName && desiredType != typeof(char).FullName
                                                       && value.Contains("\"") == false)
            {
                if (!value.StartsWith("new ") && desiredType != typeof(string).FullName)
                {
                    value = StringFunctions.RemoveWhitespace(value);
                }

                value = value.Replace("\"", "");
            }

            #region Convert To Object
            
            var handled = false;
            object toReturn = null;

            if (desiredType == typeof(string).FullName)
            {
                toReturn = value;
                handled = true;
            }

            if (!handled)
            {
                TryHandleComplexType(value, desiredType, out handled, out toReturn);
            }

            if (!handled)
            {


                #region bool

                if (desiredType == typeof(bool).FullName)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        return false;
                    }
                    
                    value = value.ToLower();

                    toReturn = bool.Parse(value);
                    handled = true;
                }

                #endregion

                #region int, Int32, Int16, uint, long

                else if (desiredType == typeof(int).FullName || desiredType == typeof(int).FullName || desiredType == typeof(short).FullName ||
                    desiredType == typeof(uint).FullName || desiredType == typeof(long).FullName || desiredType == typeof(byte).FullName)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        return 0;
                    }


                    var indexOfDecimal = value.IndexOf('.');

                    if (value.IndexOf(",", StringComparison.Ordinal) != -1)
                    {
                        value = value.Replace(",", "");
                    }

                    #region uint
#if FRB_XNA
                    if (desiredType == typeof(uint).FullName)
                    {
                        if (indexOfDecimal == -1)
                        {
                            return uint.Parse(value);

                        }
                        else
                        {
                            return (uint)(Math.MathFunctions.RoundToInt(float.Parse(value, CultureInfo.InvariantCulture)));
                        }
                    }
#endif
                    #endregion

                    #region byte
#if FRB_XNA

                    if (desiredType == typeof(byte).FullName)
                    {
                        if (indexOfDecimal == -1)
                        {
                            return byte.Parse(value);

                        }
                        else
                        {
                            return (byte)(Math.MathFunctions.RoundToInt(float.Parse(value, CultureInfo.InvariantCulture)));
                        }
                    }
#endif
                    #endregion

                    #region long
                    if (desiredType == typeof(long).FullName)
                    {
                        if (indexOfDecimal == -1)
                        {
                            return long.Parse(value);

                        }
#if FRB_XNA

                        else
                        {
                            return (long)(Math.MathFunctions.RoundToInt(float.Parse(value, CultureInfo.InvariantCulture)));
                        }
#endif
                    }

                    #endregion

                    #region regular int
                    else
                    {

                        if (indexOfDecimal == -1)
                        {
                            return int.Parse(value);

                        }
#if FRB_XNA

                        else
                        {
                            return (int)(Math.MathFunctions.RoundToInt(float.Parse(value, CultureInfo.InvariantCulture)));
                        }
#endif
                    }
                    #endregion
                }

                #endregion

                #region float, Single

                else if (desiredType == typeof(float).FullName || desiredType == typeof(Single).FullName)
                {
                    return string.IsNullOrEmpty(value) ? 0f : float.Parse(value, CultureInfo.InvariantCulture);
                }

                #endregion

                #region double

                else if (desiredType == typeof(double).FullName)
                {
                    return string.IsNullOrEmpty(value) ? 0.0 : double.Parse(value, CultureInfo.InvariantCulture);
                }

                #endregion


                #region Decimal

                else if (desiredType == typeof(decimal).FullName)
                {
                    return string.IsNullOrEmpty(value) ? 0.0m : decimal.Parse(value, CultureInfo.InvariantCulture);
                }


                #endregion


#if !FRB_RAW

                #region Texture2D

                else if (desiredType == typeof(Texture2D).FullName)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        return null;
                    }
#if !SILVERLIGHT && !ZUNE
                    if (FileManager.IsRelative(value))
                    {
                        value = FileManager.RelativeDirectory + value;
                    }
                    
                    if (FileManager.FileExists(FileManager.RemoveExtension(value) + @".xnb"))
                    {
                        var texture =
                            FlatRedBallServices.Load<Texture2D>(FileManager.RemoveExtension(value), contentManagerName);

                        
                        return texture;
                    }
                    else
                    {
                        var texture =
                            FlatRedBallServices.Load<Texture2D>(value, contentManagerName);
                        
                        return texture;
                    }
#else
                return null;
#endif
                }

                #endregion

                #region Matrix

                else if (desiredType == typeof(Matrix).FullName)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        return Matrix.Identity;
                    }

                    value = StripParenthesis(value);
                    
                    var stringvalues = value.Split(',');

                    if (stringvalues.Length != 16)
                    {
                        throw new ArgumentException(
                            $@"{Resources.StringToMatrixNeed16},{Resources.SupportStringContain} {stringvalues.Length} {
                                    Resources.Value
                                }", nameof(value));
                    }
                    
                    var values = new float[16];
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = float.Parse(stringvalues[i], CultureInfo.InvariantCulture);
                    }
                    
                    var m = new Matrix(
                        values[0], values[1], values[2], values[3],
                        values[4], values[5], values[6], values[7],
                        values[8], values[9], values[10], values[11],
                        values[12], values[13], values[14], values[15]
                        );

                    return m;
                }

                #endregion

                #region Vector2

                else if (desiredType == typeof(Vector2).FullName)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        return new Vector2(0, 0);
                    }

                    value = StripParenthesis(value);
                    
                    var stringvalues = value.Split(',');

                    if (stringvalues.Length != 2)
                    {
                        throw new ArgumentException(
                            $@"{Resources.StringToVector2Need2},{Resources.SupportStringContain} {stringvalues.Length} {
                                    Resources.Value
                                }", nameof(value));
                    }
                    
                    var values = new float[2];
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = float.Parse(stringvalues[i], CultureInfo.InvariantCulture);
                    }

                    return new Vector2(values[0], values[1]);
                }

                #endregion

                #region Vector3

                else if (desiredType == typeof(Vector3).FullName)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        return new Vector3(0, 0, 0);
                    }

                    value = StripParenthesis(value);
                    
                    var stringvalues = value.Split(',');

                    if (stringvalues.Length != 3)
                    {
                        throw new ArgumentException(
                            $@"{Resources.StringToVector3Need3},{Resources.SupportStringContain} {stringvalues.Length} {
                                    Resources.Value
                                }", nameof(value));
                    }
                    
                    var values = new float[3];
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = float.Parse(stringvalues[i], CultureInfo.InvariantCulture);
                    }

                    return new Vector3(values[0], values[1], values[2]);
                }

                #endregion

                #region Vector4

                else if (desiredType == typeof(Vector4).FullName)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        return new Vector4(0, 0, 0, 0);
                    }

                    value = StripParenthesis(value);
                    
                    var stringvalues = value.Split(',');

                    if (stringvalues.Length != 4)
                    {
                        throw new ArgumentException(
                            $@"{Resources.StringToVector4Need4},{Resources.SupportStringContain} {stringvalues.Length} {
                                    Resources.Value
                                }", nameof(value));
                    }
                    
                    var values = new float[4];
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = float.Parse(stringvalues[i], CultureInfo.InvariantCulture);
                    }

                    return new Vector4(values[0], values[1], values[2], values[3]);
                }

                #endregion
#endif
                #region enum
                else if (IsEnum(desiredType))
                {
#if DEBUG
                    if (string.IsNullOrEmpty(value))
                    {
                        throw new InvalidOperationException($"{Resources.TryCreateeEnumError}: {desiredType}");
                    }
#endif
                    
                    var foundType = MUnqualifiedTypeDictionary.ContainsKey(
                        desiredType ?? throw new ArgumentNullException(nameof(desiredType)))
                        ? MUnqualifiedTypeDictionary[desiredType]
                        : TryToGetTypeFromAssemblies(desiredType);

                    return Enum.Parse(foundType, value, true);
                }

                #endregion

                #region Color
#if FRB_XNA

                else if (desiredType == typeof(Color).FullName)
                {
#if WINDOWS_8 || UWP
                    PropertyInfo info = typeof(Color).GetProperty(value);
#else
                    PropertyInfo info = typeof(Color).GetProperty(value, BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static);
#endif

                    if (info == null)
                    {
                        if (value.StartsWith("Color."))
                        {
                            throw new Exception("Could not parse the value " + value + ".  Remove \"Color.\" and instead " +
                                "use " + value.Substring("Color.".Length));
                        }
                        else
                        {
                            throw new Exception("Could not parse " + value + " as a Color");
                        }
                    }

                    toReturn = info.GetValue(null, null);
                    handled = true;
                }

#endif
                #endregion

                #endregion
                
                if (handled) return toReturn;

                if (desiredType != null)
                    throw new NotImplementedException(
                        $"{Resources.CantConvertValue} {value} {Resources.ToType} {desiredType}");
            }

            return toReturn;
        }

        private static bool IsEnum(string typeAsString)
        {
            var foundType = MUnqualifiedTypeDictionary.ContainsKey(typeAsString)
                ? MUnqualifiedTypeDictionary[typeAsString]
                : TryToGetTypeFromAssemblies(typeAsString);

            return foundType != null &&
#if WINDOWS_8 || UWP
                foundType.IsEnum();
#else
                   foundType.IsEnum;
#endif
        }

        private static Type TryToGetTypeFromAssemblies(string typeAfterNewString)
        {
            Type foundType = null;


#if WINDOWS_8 || UWP
            foundType = TryToGetTypeFromAssembly(typeAfterNewString, FlatRedBallServices.Game.GetType().GetTypeInfo().Assembly);

            if (foundType == null)
            {
#if DEBUG
                if (TopLevelAssembly == null)
                {
                    throw new Exception("The TopLevelAssembly member must be set before it is used.  It is currently null");
                }
#endif
                foundType = TryToGetTypeFromAssembly(typeAfterNewString, TopLevelAssembly);
            }
            if (foundType == null)
            {
                foundType = TryToGetTypeFromAssembly(typeAfterNewString, typeof(Vector3).GetTypeInfo().Assembly);
            }
            if(foundType == null)
            {
                foundType = TryToGetTypeFromAssembly(typeAfterNewString, typeof(FlatRedBall.Sprite).GetTypeInfo().Assembly);
            }
#else
            
#if FRB_XNA

            if (FlatRedBallServices.Game != null)
            {
                foundType = TryToGetTypeFromAssembly(typeAfterNewString, FlatRedBallServices.Game.GetType().Assembly);
            }
#endif
            foreach (var assembly in AdditionalAssemblies)
            {
                foundType = TryToGetTypeFromAssembly(typeAfterNewString, assembly);
                if (foundType != null)
                {
                    break;
                }
            }

            if (foundType == null)
            {
                foundType = TryToGetTypeFromAssembly(typeAfterNewString, Assembly.GetExecutingAssembly());
            }
            if (foundType == null)
            {
#if WINDOWS
                foundType = TryToGetTypeFromAssembly(typeAfterNewString, Assembly.GetEntryAssembly());
#endif
            }
#if FRB_XNA

            if(foundType == null)
            {
                foundType = TryToGetTypeFromAssembly(typeAfterNewString, typeof(Vector3).Assembly);
            }
#endif
#endif

            if (foundType == null)
            {
                throw new ArgumentException("Could not find the type for " + typeAfterNewString);
            }
            else
            {
                MUnqualifiedTypeDictionary.Add(typeAfterNewString, foundType);
            }

            return foundType;
        }

        private static Type TryToGetTypeFromAssembly(string typeAfterNewString, Assembly assembly)
        {
            Type foundType = null;

            // Make sure the type isn't null, and the type string is trimmed to make the compare valid
            if (typeAfterNewString == null)
                return null;

            typeAfterNewString = typeAfterNewString.Trim();

            // Is this slow?  Do we want to cache off the Type[]?

#if WINDOWS_8
            IEnumerable<Type> types = assembly.ExportedTypes;
#else
            IEnumerable<Type> types = assembly.GetTypes();
#endif
            foreach (Type type in types)
            {
                if (type.Name == typeAfterNewString || type.FullName == typeAfterNewString)
                {
                    foundType = type;
                    break;
                }
            }

            return foundType;
        }

        private static string StripParenthesis(string value)
        {
            string result = value;

            if (result.StartsWith("("))
            {
                int startIndex = 1;
                int endIndex = result.Length - 1;
                if (result.EndsWith(")"))
                    endIndex -= 1;

                result = result.Substring(startIndex, endIndex);
            }

            return result;
        }

        private static void TryHandleComplexType(string value, string typeToConvertTo, out bool handled, out object toReturn)
        {
            handled = false;
            toReturn = null;

            if (value.StartsWith("new "))
            {
                var typeAfterNewString = value.Substring("new ".Length, value.IndexOf('(') - "new ".Length);

                var foundType = MUnqualifiedTypeDictionary.ContainsKey(typeAfterNewString)
                    ? MUnqualifiedTypeDictionary[typeAfterNewString]
                    : TryToGetTypeFromAssemblies(typeAfterNewString);

                if (foundType != null)
                {
                    var openingParen = value.IndexOf('(');

                    var closingParen = value.LastIndexOf(')');

                    if (openingParen < 0 || closingParen < 0)
                        throw new InvalidOperationException(Resources.TypeNotMatchParenthesis);

                    if (openingParen > closingParen)
                        throw new InvalidOperationException(Resources.TypeHasInCorrentParernthesis);

                    var valueInsideParens = value.Substring(openingParen + 1, closingParen - (openingParen + 1));
                    toReturn = CreateInstanceFromNamedAssignment(foundType, valueInsideParens);
                }
                else
                {
                    throw new InvalidOperationException($"{Resources.CantFindTypeInAssembly} {(Type) null}");
                }

                handled = true;
            }



            else if (value.Contains("="))
            {
                handled = true;

                var foundType = MUnqualifiedTypeDictionary.ContainsKey(typeToConvertTo)
                    ? MUnqualifiedTypeDictionary[typeToConvertTo]
                    : TryToGetTypeFromAssemblies(typeToConvertTo);

                toReturn = CreateInstanceFromNamedAssignment(foundType, value);
            }
        }

        private static object CreateInstanceFromNamedAssignment(Type type, string value)
        {
            value = value.Trim();

            var returnObject = Activator.CreateInstance(type);

            if (string.IsNullOrEmpty(value)) return returnObject;

            var split = SplitProperties(value);


            foreach (var assignment in split)
            {
                var indexOfEqual = assignment.IndexOf('=');

                if (indexOfEqual < 0)
                {
                    var message =
                        $"{Resources.InvalidValue} {assignment} {Resources.In} {value}. {Resources.ExpectedVarAssignLike} \"X={assignment}\" {Resources.WhenCreateInstance}{type.Name}";

                    throw new Exception(message);
                }
                if (indexOfEqual >= assignment.Length - 1)
                    continue;

                var variableName = assignment.Substring(0, indexOfEqual).Trim();
                var whatToassignTo = assignment.Substring(indexOfEqual + 1, assignment.Length - (indexOfEqual + 1));

                var fieldInfo = type.GetField(variableName);

                if (fieldInfo != null)
                {
                    var fieldType = fieldInfo.FieldType;
                    StripQuotesIfNecessary(ref whatToassignTo);
                    object assignValue = ConvertStringToType(whatToassignTo, fieldType);

                    fieldInfo.SetValue(returnObject, assignValue);
                }
                else
                {
                    var propertyInfo = type.GetProperty(variableName);

#if DEBUG
                    if (propertyInfo == null)
                    {
                        throw new ArgumentException(
                            $"{Resources.NotFindField} {variableName} {Resources.InType} {type.Name}");
                    }
#endif

                    var propertyType = propertyInfo.PropertyType;

                    StripQuotesIfNecessary(ref whatToassignTo);
                    object assignValue = ConvertStringToType(whatToassignTo, propertyType);

                    propertyInfo.SetValue(returnObject, assignValue, null);
                }


            }
            return returnObject;
        }

        private static void StripQuotesIfNecessary(ref string whatToassignTo)
        {
            if (whatToassignTo == null) return;

            var trimmed = whatToassignTo.Trim();

            if (trimmed.StartsWith("\"") &&
                trimmed.EndsWith("\"") && trimmed.Length > 1)
            {
                whatToassignTo = trimmed.Substring(1, trimmed.Length - 2);
            }
        }

        private static object CreateGenericListFrom(string value, Type listType, string contentManagerName)
        {
            var newObject = Activator.CreateInstance(listType);
            var genericType = listType.GetGenericArguments()[0];
            var add = listType.GetMethod("Add");

            var start = value.IndexOf("(", StringComparison.Ordinal) + 1;
            var end = value.IndexOf(")", StringComparison.Ordinal);

            if (end <= 0) return newObject;

            var insideOfParens = value.Substring(start, end - start);
                
            var values = SplitProperties(insideOfParens);

            var arguments = new object[1];

            foreach (var itemInList in values)
            {
                var converted = ConvertStringToType(itemInList, genericType, contentManagerName, true);
                arguments[0] = converted;
                if (add != null) add.Invoke(newObject, arguments);
            }

            return newObject;
        }

        public static List<string> SplitProperties(string value)
        {

            var splitOnComma = value.Split(',');

            var toReturn = new List<string>();
            
            var parenCount = 0;
            var quoteCount = 0;

            foreach (var entryInSplit in splitOnComma)
            {
                var shouldCombine = parenCount != 0 || quoteCount != 0;

                parenCount += entryInSplit.CountOf("(");
                parenCount -= entryInSplit.CountOf(")");

                quoteCount += entryInSplit.CountOf("\"");
                quoteCount = (quoteCount % 2);

                if (shouldCombine)
                {
                    toReturn[toReturn.Count - 1] = toReturn[toReturn.Count - 1] + ',' + entryInSplit;
                }
                else
                {
                    toReturn.Add(entryInSplit);
                }

            }

            return toReturn;
        }

        public static bool IsGenericList(Type type)
        {
            var isGenericList = type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>));

            return isGenericList;
        }

        public static string ConvertTypeToString(object value)
        {
            if (value == null) return string.Empty;
            
            var typeToConvertTo = value.GetType();
            
            #region Convert To String

            if (typeToConvertTo == typeof(bool))
            {
                return ((bool)value).ToString();
            }

            if (typeToConvertTo == typeof(int) || typeToConvertTo == typeof(Int32) || typeToConvertTo == typeof(Int16))
            {
                return ((int)value).ToString();
            }

            if (typeToConvertTo == typeof(float) || typeToConvertTo == typeof(Single))
            {
                return ((float)value).ToString(CultureInfo.InvariantCulture);
            }

            if (typeToConvertTo == typeof(double))
            {
                return ((double)value).ToString(CultureInfo.InvariantCulture);
            }

            if (typeToConvertTo == typeof(decimal))
            {
                return ((decimal)value).ToString(CultureInfo.InvariantCulture);
            }

            if (typeToConvertTo == typeof(string))
            {
                return (string)value;
            }

#if !FRB_RAW
            if (typeToConvertTo == typeof(Texture2D))
            {
                return ((Texture2D)value).Name;
            }

            if (typeToConvertTo == typeof(Matrix))
            {
                Matrix m = (Matrix)value;

                float[] values = new float[16];

                values[0] = m.M11;
                values[1] = m.M12;
                values[2] = m.M13;
                values[3] = m.M14;

                values[4] = m.M21;
                values[5] = m.M22;
                values[6] = m.M23;
                values[7] = m.M24;

                values[8] = m.M31;
                values[9] = m.M32;
                values[10] = m.M33;
                values[11] = m.M34;

                values[12] = m.M41;
                values[13] = m.M42;
                values[14] = m.M43;
                values[15] = m.M44;

                string outputString = string.Empty;
                
                for (int i = 0; i < values.Length; i++)
                {
                    outputString += ((i == 0) ? string.Empty : ",") +
                        ConvertTypeToString(values[i]);
                }

                return outputString;
            }

            if (typeToConvertTo == typeof(Vector2))
            {
                var v = (Vector2)value;

                return ConvertTypeToString(v.X) + "," +
                    ConvertTypeToString(v.Y);
            }

            if (typeToConvertTo == typeof(Vector3))
            {
                var v = (Vector3)value;

                return ConvertTypeToString(v.X) + "," +
                    ConvertTypeToString(v.Y) + "," +
                    ConvertTypeToString(v.Z);
            }

            if (typeToConvertTo == typeof(Vector4))
            {
                var v = (Vector4)value;

                return ConvertTypeToString(v.X) + "," +
                    ConvertTypeToString(v.Y) + "," +
                    ConvertTypeToString(v.Z) + "," +
                    ConvertTypeToString(v.W);
            }
#endif
#if UWP
            if (typeToConvertTo.IsEnum())
#else
            if (typeToConvertTo.IsEnum)
#endif
            {
                return value.ToString();
            }

            #endregion
            
            return string.Empty;
        }
    }
}