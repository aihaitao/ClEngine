using Microsoft.Build.Evaluation;

namespace ClEngine.CoreLibrary.Projects
{
    public class AndroidProject : CombinedEmbeddedContentProject
    {
        public AndroidProject(Project mProject) : base(mProject)
        {
        }
    }
}