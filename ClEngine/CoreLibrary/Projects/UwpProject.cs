using Microsoft.Build.Evaluation;

namespace ClEngine.CoreLibrary.Projects
{
    public class UwpProject : CombinedEmbeddedContentProject
    {
        public UwpProject(Project project) : base(project)
        {
        }
    }
}