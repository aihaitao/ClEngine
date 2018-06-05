using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ClEngine.View.Project;
using FlatRedBall.IO;
using Microsoft.Build.Evaluation;
using NewProjectCreator.Views;

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

        public override void Load(string fileName)
        {
            mBuildItemDictionaries.Clear();

            bool wasChanged = false;

            #region Build the mBuildItemDictionary to make accessing items faster
            for (int i = mProject.AllEvaluatedItems.Count - 1; i > -1; i--)
            {
                ProjectItem buildItem = mProject.AllEvaluatedItems.ElementAt(i);

                string includeToLower = buildItem.EvaluatedInclude.ToLower();

                if (buildItem.IsImported)
                {
                    //Do Nothing

                    // 04-12-2012
                    // Some computers add a duplicate mscorlib when loading the core Project. Glue would catch this and remove it, 
                    // but for some reason this would remove all instances...causing problems when loading the project. We aren't 
                    // sure why, but if we check for the isIncluded flag it seems to fix it, and isIncluded doesn't seem to be true 
                    // on things added by Glue - and that's ultimately what we want to check duplicates on.
                }
                else if (mBuildItemDictionaries.ContainsKey(includeToLower))
                {
                    wasChanged = ResolveDuplicateProjectEntry(wasChanged, buildItem);
                }
                else
                {
                    mBuildItemDictionaries.Add(
                        buildItem.UnevaluatedInclude.ToLower(),
                        buildItem);
                }
            }
            #endregion

            FindRootNamespace();

            // December 20, 2010

            if (wasChanged)
            {
                mProject.Save(mProject.ProjectFileLocation.File);
            }

            LoadContentProject();
        }

        private bool ResolveDuplicateProjectEntry(bool wasChanged, ProjectItem buildItem)
        {

            MultiButtonMessageBox mbmb = new MultiButtonMessageBox();

            mbmb.MessageText = "The item " + buildItem.UnevaluatedInclude + " is part of " +
                "the project twice.  Glue does not support double-entries in a project.  What would you like to do?";

            mbmb.AddButton("Remove the duplicate entry and continue", DialogResult.OK);
            mbmb.AddButton("Remove the duplicate, but show me a list of all contained objects before removal", DialogResult.No);
            mbmb.AddButton("Cancel loading the project - this will throw an exception", DialogResult.Cancel);

            DialogResult result;

            if (Gui.TryShowDialog(mbmb, out result))
            {
                switch (result)
                {
                    case DialogResult.OK:
                        mProject.RemoveItem(buildItem);
                        mProject.ReevaluateIfNecessary();
                        break;
                    case DialogResult.No:
                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (var item in mProject.AllEvaluatedItems)
                        {
                            stringBuilder.AppendLine(item.ItemType + " " + item.UnevaluatedInclude);
                        }
                        string whereToSave = FileManager.UserApplicationDataForThisApplication + "ProjectFileOutput.txt";
                        FileManager.SaveText(stringBuilder.ToString(), whereToSave);
                        Process.Start(whereToSave);


                        mProject.RemoveItem(buildItem);
                        mProject.ReevaluateIfNecessary();
                        break;
                    case DialogResult.Cancel:
                        throw new Exception("Duplicate entries found: " + buildItem.ItemType + " " + buildItem.UnevaluatedInclude);
                }

            }
            else
            {
                //mProject.EvaluatedItems.RemoveItemAt(i);
                mProject.RemoveItem(buildItem);
                mProject.ReevaluateIfNecessary();
            }
            wasChanged = true;
            return wasChanged;
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