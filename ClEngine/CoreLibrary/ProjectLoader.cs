using System.IO;
using ClEngine.Properties;
using ClEngine.View.Project;
using FlatRedBall;
using FlatRedBall.Performance.Measurement;

namespace ClEngine.CoreLibrary
{
    public class ProjectLoader
    {
        public void LoadProject(string projectFileName)
        {
            LoadProject(projectFileName, null);
        }

        public void LoadProject(string projectFileName, InitializationWindow initializationWindow)
        {
            TimeManager.Initialize();
            var topSection = Section.GetAndStartContextAndTime("All");
            if (!File.Exists(projectFileName))
            {
                Gui.ShowException($"{Resources.CantFindProject}{projectFileName}\n\n{Resources.OpenWithoutProject}",
                    Resources.ErrorLoadProject, null);
                return;
            }
            
        }
    }
}