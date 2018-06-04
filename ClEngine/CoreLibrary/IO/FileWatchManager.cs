namespace ClEngine.CoreLibrary.IO
{
    internal static class FileWatchManager
    {
        internal static ChangedFileGroup mChangedProjectFiles;
        public static void IgnoreNextChangeOnFile(string file)
        {
#if !UNIT_TESTS
            mChangedProjectFiles.IgnoreNextChangeOn(file);
#endif
        }
    }
}