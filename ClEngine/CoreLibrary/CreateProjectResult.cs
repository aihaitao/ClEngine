using ClEngine.CoreLibrary.Build;

namespace ClEngine.CoreLibrary
{
    public class CreateProjectResult
    {
        public ProjectBase Project { get; set; }
        public bool ShouldTryLoadProject { get; set; } = true;
    }
}