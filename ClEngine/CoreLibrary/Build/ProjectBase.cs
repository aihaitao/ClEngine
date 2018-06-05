using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FlatRedBall.IO;
using Microsoft.Build.Evaluation;

namespace ClEngine.CoreLibrary.Build
{
    public delegate void SaveDelegate(string fileName);
    public abstract class ProjectBase : IEnumerable<ProjectItem>
    {
        protected Dictionary<string, ProjectItem> BuildItemDictionaries = new Dictionary<string, ProjectItem>();
        public event SaveDelegate Saving;

        protected Dictionary<string, ProjectItem> mBuildItemDictionaries =
            new Dictionary<string, ProjectItem>();

        public ProjectBase ContentProject { get; set; }

        public string Directory => FileManager.GetDirectory(FullFileName);

        public virtual string ContentDirectory => "";

        public abstract string Name { get; }

        public bool IsContentProject
        {
            get;
            set;
        }

        public virtual string RootNamespace => Name;

        public abstract void Save(string fileName);

        public abstract string FullFileName
        {
            get;
        }

        protected void RaiseSaving(string fileName)
        {
            Saving?.Invoke(fileName);
        }

        public abstract void Load(string fileName);

        public IEnumerator<ProjectItem> GetEnumerator()
        {
            return BuildItemDictionaries.Values.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return BuildItemDictionaries.Values.GetEnumerator();
        }

        public string GetAbsoluteContentFolder()
        {
            if (ContentProject != this)
            {
                return ContentProject.Directory;
            }

            if (!string.IsNullOrEmpty(ContentProject.ContentDirectory))
            {
                return Directory + ContentDirectory;
            }

            return Directory;
        }

        public virtual void LoadContentProject()
        {
            ContentProject = this;
        }

        public string MakeAbsolute(string relativePath)
        {
            if (FileManager.IsRelative(relativePath))
            {
                string lowerCase = relativePath.ToLower();

                if (this.IsContentProject &&
                    (lowerCase.StartsWith("content/") || lowerCase.StartsWith(@"content\")))
                {
                    relativePath = relativePath.Substring("content/".Length, relativePath.Length - "content/".Length);
                }

                string returnValue = this.Directory + relativePath;

                if (System.IO.Directory.Exists(returnValue) &&
                    !returnValue.EndsWith("/") &&
                    !returnValue.EndsWith("\\"))
                {
                    returnValue += "/";
                }

                returnValue = FileManager.Standardize(returnValue);

                return returnValue;
            }
            else
            {
                return relativePath;
            }
        }
    }
}