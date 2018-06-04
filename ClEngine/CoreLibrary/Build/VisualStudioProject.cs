using ClEngine.CoreLibrary.IO;
using Microsoft.Build.Evaluation;

namespace ClEngine.CoreLibrary.Build
{
    public abstract class VisualStudioProject : ProjectBase
    {
        internal Project Project;
        internal new string RootNamespace;
        private string _name;

        public override string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                    _name = FileManager.RemoveExtension(FileManager.RemovePath(Project.ProjectFileLocation.File));

                return _name;
            }
        }

        protected VisualStudioProject(Project project)
        {
            Project = project;

            FindRootNamespace();
        }

        private void FindRootNamespace()
        {
            RootNamespace = base.RootNamespace;
            if (Project != null)
            {
                foreach (var projectProperty in Project.Properties)
                {
                    if (projectProperty.Name == "RootNamespace")
                    {
                        RootNamespace = projectProperty.EvaluatedValue;
                        break;
                    }
                }
            }
        }
    }
}