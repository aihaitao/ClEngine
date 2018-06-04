using System;

namespace ClEngine.CoreLibrary.Utilities
{
    public static class StringFunctions
    {
        public static string RemoveWhitespace(string stringToRemoveWhitespaceFrom)
        {
            return stringToRemoveWhitespaceFrom.Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
        }

        public static int CountOf(this string instanceToSearchIn, string whatToFind)
        {
            var count = 0;
            var index = 0;
            while (true)
            {
                var foundIndex = instanceToSearchIn.IndexOf(whatToFind, index, StringComparison.Ordinal);

                if (foundIndex != -1)
                {
                    count++;

                    index = foundIndex + 1;
                }
                else
                {
                    break;
                }
            }

            return count;

        }
    }
}