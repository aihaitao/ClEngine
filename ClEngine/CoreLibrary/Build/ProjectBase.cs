using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Evaluation;

namespace ClEngine.CoreLibrary.Build
{
    public delegate void SaveDelegate(string fileName);
    public abstract class ProjectBase : IEnumerable<ProjectItem>
    {
        protected Dictionary<string, ProjectItem> BuildItemDictionaries = new Dictionary<string, ProjectItem>();
        public event SaveDelegate Saving;

        public ProjectBase ContentProject { get; set; }

        public abstract string Name { get; }

        public virtual string RootNamespace => Name;

        public IEnumerator<ProjectItem> GetEnumerator()
        {
            return BuildItemDictionaries.Values.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return BuildItemDictionaries.Values.GetEnumerator();
        }
    }
}