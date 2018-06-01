using System.Collections.Generic;
using System.IO;
using ClEngine.Core.Properties;
using FlatRedBall.IO;
using FlatRedBall.IO.Csv;
using Xceed.Wpf.Toolkit;

namespace ClEngine.Core.ProjectCreator
{
    public class DataLoader
    {
        internal static List<PlatformProjectInfo> _starterProjects = new List<PlatformProjectInfo>();
        public static List<PlatformProjectInfo> StarterProjects => _starterProjects;

        internal static List<PlatformProjectInfo> _emptyProjects = new List<PlatformProjectInfo>();
        public static List<PlatformProjectInfo> EmptyProjects => _emptyProjects;

        public static void LoadAvaliableProjectFromCsv()
        {
            var fileName = "Content/EmptyTemplates.csv";

            var absoluteFile = FileManager.RelativeDirectory + fileName;

            if (!File.Exists(absoluteFile))
                MessageBox.Show($"{Resources.LoadProjectTip1}{absoluteFile}{Resources.LoadProjectTip2}");
            else
            {
                EmptyProjects.Clear();
                CsvFileManager.CsvDeserializeList(typeof(PlatformProjectInfo), fileName, EmptyProjects);
            }


            fileName = "Content/StarterProjects.csv";

            absoluteFile = FileManager.RelativeDirectory + fileName;

            if (!File.Exists(absoluteFile))
                MessageBox.Show($"{Resources.LoadProjectTip3}{absoluteFile}{Resources.LoadProjectTip4}");
            else
            {
                StarterProjects.Clear();
                CsvFileManager.CsvDeserializeList(typeof(PlatformProjectInfo), fileName, StarterProjects);
            }
        }
    }
}