using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FlatRedBall.IO;

namespace ClEngine.Core.ProjectCreator
{
    public class EmbeddedExecutableExtractor
    {
        internal static EmbeddedExecutableExtractor mSelf;

        public static EmbeddedExecutableExtractor Self => mSelf ?? (mSelf = new EmbeddedExecutableExtractor());

        public string ExtractFile(string unqualifiedName)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var saveAsName = FileManager.UserApplicationDataForThisApplication + unqualifiedName;
            ExtractFileFromAssembly(currentAssembly, unqualifiedName, saveAsName);
            return saveAsName;
        }

        public bool ExtractFileFromAssembly(Assembly currentAssembly, string unqualifiedName, string saveAsName)
        {
            var arrResources = currentAssembly.GetManifestResourceNames();
            var qualifiedName = arrResources.FirstOrDefault(arrResource => arrResource.EndsWith("." + unqualifiedName));

            var fileInfoOutputFile = new FileInfo(saveAsName);


            using (var streamToOutputFile = fileInfoOutputFile.OpenWrite())
            {
                using (var streamToResourceFile = currentAssembly.GetManifestResourceStream(qualifiedName))
                {
                    const int size = 4096;
                    var bytes = new byte[4096];
                    int numBytes;
                    while (streamToResourceFile != null && (numBytes = streamToResourceFile.Read(bytes, 0, size)) > 0)
                    {
                        streamToOutputFile.Write(bytes, 0, numBytes);
                    }
                }
            }

            if (!File.Exists(saveAsName))
                throw new Exception("NotExtractedCorrectly".GetTranslateName());

            return true;
        }
    }
}