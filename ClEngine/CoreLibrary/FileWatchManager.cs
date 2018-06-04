using System.Collections.Generic;
using ClEngine.CoreLibrary.IO;

namespace ClEngine.CoreLibrary
{
    internal static class FileWatchManager
    {
        static ChangedFileGroup mChangedProjectFiles;
        static ChangeInformation BuiltFileChangeInfo = new ChangeInformation();
        
        public static bool PerformFlushing = true;

        static bool IsFlushing;
        
        public static object LockObject = new object();

        #region Methods

        public static void Initialize()
        {
            mChangedProjectFiles = new ChangedFileGroup();
            
            Dictionary<string, int> mChangesToIgnore = new Dictionary<string, int>();
            mChangedProjectFiles.SetIgnoreDictionary(mChangesToIgnore);
            mChangedProjectFiles.SortDelegate = CompareFiles;
        }

        public static void IgnoreNextChangeOnFile(string file)
        {
#if !UNIT_TESTS
            mChangedProjectFiles.IgnoreNextChangeOn(file);
#endif
        }

        private static int CompareFiles(string first, string second)
        {
            int firstValue = GetFileValue(first);
            int secondValue = GetFileValue(second);

            return secondValue - firstValue;
        }


        static int GetFileValue(string file)
        {
            // CSProj files first

            string extension = FileManager.GetExtension(file);
            if (extension == "csproj" ||
                extension == "vcproj")
            {
                return 3;
            }
            else if (extension == "contentproj")
            {
                return 2;
            }
            else if (extension == "glux")
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        #endregion
    }
}