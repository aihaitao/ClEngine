using System.IO;
using System.Text;
using FlatRedBall.IO;
using Microsoft.Build.Evaluation;

namespace ClEngine.CoreLibrary.Build
{
    public abstract class VisualStudioProject : ProjectBase
    {
        internal Project mProject;
        internal new string RootNamespace;
        private string _name;

        public override string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                    _name = FileManager.RemoveExtension(FileManager.RemovePath(mProject.ProjectFileLocation.File));

                return _name;
            }
        }

        public override string FullFileName => mProject.ProjectFileLocation.File;

        protected VisualStudioProject(Project mProject)
        {
            this.mProject = mProject;

            FindRootNamespace();
        }

        private void FindRootNamespace()
        {
            RootNamespace = base.RootNamespace;
            if (mProject != null)
            {
                foreach (var projectProperty in mProject.Properties)
                {
                    if (projectProperty.Name == "RootNamespace")
                    {
                        RootNamespace = projectProperty.EvaluatedValue;
                        break;
                    }
                }
            }
        }

        public override void Save(string fileName)
        {
            var shouldSave = false;

            using (var stringWriter = new Utf8StringWriter())
            {
                mProject.ReevaluateIfNecessary();

                mProject.Save(stringWriter);

                var newText = stringWriter.ToString();
                var oldText = File.ReadAllText(fileName);

                if (oldText != newText)
                {
                    RaiseSaving(FullFileName);

                    File.WriteAllText(FullFileName, newText, stringWriter.Encoding);
                }
            }
        }
    }

    public sealed class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}