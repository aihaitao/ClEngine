using System.IO;
using System.Linq;
using ClEngine.Core.Properties;
using FlatRedBall.IO;
using Ionic.Zip;
using Xceed.Wpf.Toolkit;

namespace ClEngine.Core.ProjectCreator
{
    public class UnzipManager
    {
        public static bool UnzipFile(string zipToUnpack, string unpackDirectory)
        {
            var succeeded = true;

            succeeded = Unzip(zipToUnpack, unpackDirectory, succeeded);

            if (succeeded)
            {
                var isGithubZip = GetIfIsGithubZip(unpackDirectory);

                if (isGithubZip)
                {
                    CopyContentsUpOneDirectory(unpackDirectory);
                }
            }


            return succeeded;
        }

        private static void CopyContentsUpOneDirectory(string unpackDirectory)
        {
            var directories = Directory.GetDirectories(unpackDirectory);

            var directoryToMove = directories.First();

            FileManager.CopyDirectory(directoryToMove, unpackDirectory, false);

            FileManager.DeleteDirectory(directoryToMove);
        }

        private static bool GetIfIsGithubZip(string unpackDirectory)
        {
            var files = Directory.GetFiles(unpackDirectory);
            var directories = Directory.GetDirectories(unpackDirectory);

            return !files.Any() && directories.Count() == 1;
        }

        private static bool Unzip(string zipToUnpack, string unpackDirectory, bool succeeded)
        {
            try
            {

                using (var zip1 = ZipFile.Read(zipToUnpack))
                {
                    // here, we extract every entry, but we could extract conditionally
                    // based on entry name, size, date, checkbox status, etc.  
                    foreach (var zipEntry in zip1)
                    {
                        zipEntry.Extract(unpackDirectory, ExtractExistingFileAction.OverwriteSilently);
                    }
                }
            }
            catch
            {
                succeeded = false;
                MessageBox.Show(Resources.NotUnzippedDeleteFileAndReDownload);
                File.Delete(zipToUnpack);
            }

            return succeeded;
        }
    }
}