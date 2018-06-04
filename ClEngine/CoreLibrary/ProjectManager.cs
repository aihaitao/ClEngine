using ClEngine.CoreLibrary.Build;
using ClEngine.CoreLibrary.Elements;
using ClEngine.CoreLibrary.Interfaces;
using ClEngine.CoreLibrary.SaveClasses;
// ReSharper disable UnusedMember.Global

namespace ClEngine.CoreLibrary
{
    public class ProjectManager : IVsProjectState
    {
        public enum CheckResult
        {
            Passed,
            Failed
        }

        internal static ProjectBase MProjectBase;

        public string DefaultNamespace => ProjectNamespace;

        public static string ProjectNamespace
        {
            get
            {
#if TEST
                return "TestProjectNamespace";
#else
                return MProjectBase?.RootNamespace;
#endif
            }
        }

        internal static ProjectSave MProjectSave;
        public static ProjectSave ProjectSave
        {
            get => MProjectSave;
            internal set
            {
                MProjectSave = value;
                ObjectFinder.Self.Project = MProjectSave;
            }
        }

        public static CheckResult StatusCheck()
        {
            return CheckResult.Passed;
        }

        public static ProjectBase ContentProject => MProjectBase?.ContentProject;
    }
}