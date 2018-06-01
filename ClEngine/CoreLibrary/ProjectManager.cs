using FlatRedBall.Glue.VSHelpers.Projects;
using GluePropertyGridClasses.Interfaces;

namespace ClEngine.CoreLibrary
{
    public class ProjectManager : IVsProjectState
    {
        internal static ProjectBase mProjectBase;

        public string DefaultNamespace => ProjectNamespace;

        public static string ProjectNamespace
        {
            get
            {
#if TEST
                return "TestProjectNamespace";
#else
                return mProjectBase?.RootNamespace;
#endif
            }
        }

        public static ProjectBase ContentProject => mProjectBase?.ContentProject;
    }
}