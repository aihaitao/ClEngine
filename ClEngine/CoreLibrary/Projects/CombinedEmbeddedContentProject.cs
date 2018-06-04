using ClEngine.CoreLibrary.Build;
using Microsoft.Build.Evaluation;

namespace ClEngine.CoreLibrary.Projects
{
    public abstract class CombinedEmbeddedContentProject : VisualStudioProject
    {
        protected CombinedEmbeddedContentProject(Project mProject) : base(mProject)
        {
        }
        
    }
}