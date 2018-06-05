using System;
using System.Collections.Generic;
using System.Linq;
using ClEngine.CoreLibrary.Build;
using ClEngine.CoreLibrary.Elements;
using ClEngine.CoreLibrary.Interfaces;
using ClEngine.CoreLibrary.Plugins;
using ClEngine.CoreLibrary.SaveClasses;
using ClEngine.Properties;
using FlatRedBall.Glue.SaveClasses;
using FlatRedBall.IO;
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

        public static string ContentDirectory => ProjectBase?.GetAbsoluteContentFolder();

        public static bool WantsToClose { get; set; }

        public static SettingsSave SettingsSave { get; set; } = new SettingsSave();

        internal static ProjectBase mProjectBase;

        public static ProjectBase ProjectBase
        {
            get => mProjectBase;
            set => mProjectBase = value;
        }

        public static string ContentDirectoryRelative => ContentProject == null ? "" : ContentProject.ContentDirectory;

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

        public static string MakeRelativeContent(string relativePath)
        {
            if (!FileManager.IsRelative(relativePath))
            {
                return ContentProject != null
                    ? FileManager.MakeRelative(relativePath, ContentDirectory)
                    : FileManager.MakeRelative(relativePath);
            }

            return FileManager.MakeRelative(relativePath);

        }

        public static CheckResult StatusCheck()
        {
            return CheckResult.Passed;
        }

        public static ProjectBase ContentProject => mProjectBase?.ContentProject;

        public static string MakeAbsolute(string relativePath, bool forceAsContent)
        {
            if (!FileManager.IsRelative(relativePath)) return relativePath;

            if (forceAsContent || IsContent(relativePath))
            {
                return !relativePath.StartsWith(ContentDirectoryRelative)
                    ? ContentProject.MakeAbsolute(ContentDirectoryRelative + relativePath)
                    : ContentProject.MakeAbsolute(relativePath);
            }

            return ProjectBase.MakeAbsolute(relativePath);

        }

        public static bool IsContent(string file)
        {
            var extension = FileManager.GetExtension(file);

            if (extension == "")
            {
                return false;
            }

            if (AvailableAssetTypes.Self.AllAssetTypes.Any(ati => ati.Extension == extension))
            {
                return true;
            }

            if (AvailableAssetTypes.Self.AdditionalExtensionsToTreatAsAssets.Contains(extension))
            {
                return true;
            }

            if (PluginManager.CanFileReferenceContent(file))
            {
                return true;
            }

            return extension == "csv" || extension == "xml";
        }

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