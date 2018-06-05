using System.IO;
using System.Linq;
using FlatRedBall.IO;

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

        public static bool PerformFlushing = true;

        public static void UpdateToProjectDirectory()
        {
            if (ProjectManager.ProjectBase != null && !string.IsNullOrEmpty(ProjectManager.ProjectBase.FullFileName))
            {
                var directory = FileManager.GetDirectory(ProjectManager.ProjectBase.FullFileName);

                directory = FileManager.GetDirectory(directory);
                while (ShouldMoveUpForRoot(directory))
                {
                    directory = FileManager.GetDirectory(directory);
                }

                if (mChangedProjectFiles == null) return;

                mChangedProjectFiles.Path = directory;
                mChangedProjectFiles.Enabled = true;
            }
            else
            {
                if (mChangedProjectFiles != null)
                {
                    mChangedProjectFiles.Enabled = false;
                }
            }
        }

        private static bool ShouldMoveUpForRoot(string directory)
        {
            var folderName = FileManager.RemovePath(directory).Replace("/", "").Replace("\\", "");

            var returnValue = folderName == ProjectManager.ProjectBase.Name &&
                              string.IsNullOrEmpty(Directory.GetFiles(directory)
                                  .FirstOrDefault(s => FileManager.GetExtension(s) == "sln"));
            return returnValue;
        }
    }
}