using ClEngine.CoreLibrary.Build;
using Microsoft.Build.Evaluation;

namespace ClEngine.CoreLibrary.Projects
{
    public abstract class CombinedEmbeddedContentProject : VisualStudioProject
    {
        protected CombinedEmbeddedContentProject(Project project) : base(project)
        {
        }
        
    }
}