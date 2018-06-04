using System;
using System.Collections.Generic;
using ClEngine.CoreLibrary.Build;
using ClEngine.CoreLibrary.Elements;
using ClEngine.CoreLibrary.Interfaces;
using ClEngine.CoreLibrary.SaveClasses;
using ClEngine.Properties;
using FlatRedBall.Glue.SaveClasses;
using Xceed.Wpf.Toolkit;

// ReSharper disable UnusedMember.Global

namespace ClEngine.CoreLibrary
{
    public class ProjectManager : IVsProjectState
    {
        internal static List<ProjectBase> mSyncedProjects = new List<ProjectBase>();
        public enum CheckResult
        {
            Passed,
            Failed
        }

        public static bool WantsToClose { get; set; }

        public static SettingsSave SettingsSave { get; set; } = new SettingsSave();

        internal static ProjectBase mProjectBase;

        public static ProjectBase ProjectBase
        {
            get => mProjectBase;
            set => mProjectBase = value;
        }

        public string DefaultNamespace => ProjectNamespace;

        public static string ProjectNamespace
        {
            get
            {
#if TEST
                return "TestProjectNamespace";
#else
                return mProjectBase?.RootNamespace;
#endif
            }
        }

        internal static GlueProjectSave MProjectSave;
        public static GlueProjectSave ProjectSave
        {
            get => MProjectSave;
            internal set
            {
                MProjectSave = value;
                ObjectFinder.Self.Project = MProjectSave;
            }
        }

        public static CheckResult StatusCheck()
        {
            return CheckResult.Passed;
        }

        public static ProjectBase ContentProject => mProjectBase?.ContentProject;

        public static void SaveProjects()
        {
            lock (mProjectBase)
            {
                var shouldSync = false;

                if (mProjectBase != null)
                {
                    var succeeded = true;
                    try
                    {
                        mProjectBase.Save(mProjectBase.FullFileName);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show(Resources.CantSaveFileInUse);
                        succeeded = false;
                    }

                    if (succeeded)
                    {
                        shouldSync = true;
                    }
                }

                if (ContentProject != null && ContentProject != mProjectBase)
                {
                    ContentProject.Save(ContentProject.FullFileName);
                    shouldSync = true;
                }

                foreach (var mSyncedProject in mSyncedProjects)
                {
                    try
                    {
                        mSyncedProject.Save(mSyncedProject.FullFileName);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }
    }
}