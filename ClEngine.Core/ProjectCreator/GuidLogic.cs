using System;
using ClEngine.Core.Properties;
using FlatRedBall.IO;
using FlatRedBall.Utilities;

namespace ClEngine.Core.ProjectCreator
{
    public static class GuidLogic
    {
        public static void ReplaceGuids(string unpackDirectory)
        {
            var newGuid = Guid.NewGuid().ToString();
            var oldGuid = GetOldGuid(unpackDirectory);

            ReplaceGuidInFile(oldGuid, newGuid, unpackDirectory, "csproj");
            ReplaceGuidInFile(oldGuid, newGuid, unpackDirectory, "sln");

            ReplaceAssemblyInfoGuids(unpackDirectory, newGuid);
        }

        private static void ReplaceAssemblyInfoGuids(string unpackDirectory, string newGuid)
        {
            var stringList = FileManager.GetAllFilesInDirectory(unpackDirectory, "cs");

            foreach (var s in stringList)
            {
                if (s.ToLower().Contains("assemblyinfo.cs"))
                {
                    var contents = FileManager.FromFileText(s);

                    var newLine = "[assembly: Guid(\"" + newGuid + "\")]";
                    StringFunctions.ReplaceLine(ref contents, "[assembly: Guid(", newLine);
                    FileManager.SaveText(contents, s);
                }
            }
        }

        private static void ReplaceGuidInFile(string oldGuid, string newGuid, string unpackDirectory, string extension)
        {
            var fileName = GetSingleFileByExtension(unpackDirectory, extension);

            var contents = FileManager.FromFileText(fileName);

            contents = contents.Replace(oldGuid.ToLowerInvariant(), newGuid.ToLowerInvariant());
            contents = contents.Replace(oldGuid.ToUpperInvariant(), newGuid.ToUpperInvariant());

            FileManager.SaveText(contents, fileName);
        }

        private static string GetOldGuid(string unpackDirectory)
        {
            var csproj = GetCsproj(unpackDirectory);

            return GetGuid(csproj);
        }

        public static string GetGuid(string projectLocation)
        {
            var projectContents = FileManager.FromFileText(projectLocation);
            var startIndex = projectContents.IndexOf("<ProjectGuid>{", StringComparison.Ordinal) + "<ProjectGuid>{".Length;
            var endIndex = projectContents.IndexOf("</ProjectGuid>", StringComparison.Ordinal);
            projectContents = projectContents.Substring(startIndex, endIndex - startIndex);
            return projectContents;
        }

        private static string GetCsproj(string unpackDirectory)
        {
            var extension = "csproj";

            return GetSingleFileByExtension(unpackDirectory, extension);
        }

        private static string GetSingleFileByExtension(string unpackDirectory, string extenison)
        {
            var stringList = FileManager.GetAllFilesInDirectory(unpackDirectory, extenison);

            if (stringList.Count > 1)
                throw new Exception($"{Resources.MultiLocation}.{extenison}{Resources.GuidCantReplace}");

            if (stringList.Count == 0)
                throw new Exception($"{Resources.CantFindAny}.{extenison}{Resources.Files}");

            var fullFileName = stringList[0];
            return fullFileName;
        }
    }
}