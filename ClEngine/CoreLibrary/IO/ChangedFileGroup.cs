﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using FlatRedBall.IO;

namespace ClEngine.CoreLibrary.IO
{
    internal class ChangeInformation
    {
        internal static TimeSpan mMinimumTimeAfterChangeToReact = new TimeSpan(0, 0, 1);

        internal List<string> mChangedFiles = new List<string>();

        public bool CanFlush => DateTime.Now - LastAdd > mMinimumTimeAfterChangeToReact;

        public DateTime LastAdd
        {
            get;
            private set;
        }

        public ReadOnlyCollection<string> Changes
        {
            get;
            private set;
        }

        public ChangeInformation()
        {
            Changes = new ReadOnlyCollection<string>(mChangedFiles);
        }

        public void Add(string fileName)
        {
            fileName = FileManager.Standardize(fileName);

            if (!mChangedFiles.Contains(fileName))
            {
                mChangedFiles.Add(fileName);
            }

            LastAdd = DateTime.Now;
        }

        public void Clear()
        {
            mChangedFiles.Clear();
        }

        public void Sort(Comparison<string> comparison)
        {
            mChangedFiles.Sort(comparison);
        }
    }

    public class ChangedFileGroup
    {
        #region Fields

        Dictionary<string, int> mChangesToIgnore;

        FileSystemWatcher mFileSystemWatcher;


        static TimeSpan mMinimumTimeAfterChangeToReact = new TimeSpan(0, 0, 1);

        ChangeInformation mChangedFiles;
        ChangeInformation mDeletedFiles;

        DateTime mLastModification;

        #endregion

        #region Properties

        public bool CanFlush => mChangedFiles.CanFlush && mDeletedFiles.CanFlush;

        public object LockObject
        {
            get;
            private set;
        }

        public bool Enabled
        {
            get => mFileSystemWatcher.EnableRaisingEvents;
            set => mFileSystemWatcher.EnableRaisingEvents = value;
        }

        public string Path
        {
            get => mFileSystemWatcher.Path;
            set => mFileSystemWatcher.Path =
                value?.EndsWith("/") == true ? value.Substring(0, value.Length - 1) : value;
        }

        public ReadOnlyCollection<string> DeletedFiles => mDeletedFiles.Changes;

        public ReadOnlyCollection<string> ChangedFiles => mChangedFiles.Changes;

        public IEnumerable<string> AllFiles
        {
            get
            {
                var toReturn = new List<string>();
                lock (LockObject)
                {
                    toReturn.AddRange(mDeletedFiles.Changes);
                    toReturn.AddRange(mChangedFiles.Changes);
                }
                return toReturn;
            }
        }

        #endregion

        public Comparison<string> SortDelegate;

        public ChangedFileGroup()
        {
            mChangesToIgnore = new Dictionary<string, int>();
            LockObject = new object();
            mFileSystemWatcher = new FileSystemWatcher();

            mChangedFiles = new ChangeInformation();
            mDeletedFiles = new ChangeInformation();

            mFileSystemWatcher.Filter = "*.*";
            mFileSystemWatcher.IncludeSubdirectories = true;
            mFileSystemWatcher.NotifyFilter =
                NotifyFilters.LastWrite |
                // tiled seems to save the file with a temp name like
                // MyFile.tmx.D1234
                // then it renames it to
                // MyFile.tmx
                // We need to handle the filename changing or else Glue isn't notified of the change.
                // Update - only do this for TMX (see below).
                // Update 2 - We need to handle renames incase
                //            the user renamed files to solve missing
                //            file errors. Just don't handle the .glux
                //            so we don't get double-loads.
                NotifyFilters.FileName |
                NotifyFilters.DirectoryName;


            mFileSystemWatcher.Deleted += new FileSystemEventHandler(HandleFileSystemDelete);
            mFileSystemWatcher.Changed += new FileSystemEventHandler(HandleFileSystemChange);
            mFileSystemWatcher.Renamed += HandleRename;
        }

        private void HandleRename(object sender, RenamedEventArgs e)
        {

            var shouldProcess = FileManager.GetExtension(e.Name) != "glux";

            if (shouldProcess)
            {
                // Process both the old and the new just in case someone depended on the old
                AddChangedFileTo(e.OldFullPath, mChangedFiles);
                AddChangedFileTo(e.FullPath, mChangedFiles);
            }

        }

        public void ClearIgnores()
        {
            mChangesToIgnore.Clear();
        }

        public int NumberOfTimesToIgnore(string file)
        {

            if (FileManager.IsRelative(file))
            {
                throw new Exception("File name should be absolute");
            }
            string standardized = FileManager.Standardize(file, null, false).ToLower();

            if (mChangesToIgnore.ContainsKey(standardized))
            {
                return mChangesToIgnore[standardized];
            }
            else
            {
                return 0;
            }
        }

        public void SetIgnoreDictionary(Dictionary<string, int> sharedInstance)
        {
            mChangesToIgnore = sharedInstance;
        }

        void HandleFileSystemDelete(object sender, FileSystemEventArgs e)
        {
            string fileName = e.FullPath;
            var toAddTo = mDeletedFiles;

            bool shouldIgnoreDelete = GetIfShouldIgnoreDelete(fileName);
            if (!shouldIgnoreDelete)
            {
                AddChangedFileTo(fileName, toAddTo);
            }

        }

        private bool GetIfShouldIgnoreDelete(string fileName)
        {
            var extension = FileManager.GetExtension(fileName);
            return extension == "glux";
        }

        void HandleFileSystemChange(object sender, FileSystemEventArgs e)
        {
            string fileName = e.FullPath;

            bool shouldProcess = true;

            if (e.ChangeType == WatcherChangeTypes.Renamed)
            {
                // don't process the .glux file, that gets renamed all the time
                shouldProcess = FileManager.GetExtension(fileName) != "glux";
            }

            if (shouldProcess)
            {
                ChangeInformation toAddTo = mChangedFiles;

                bool wasAdded = AddChangedFileTo(fileName, toAddTo);
            }
        }

        private bool AddChangedFileTo(string fileName, ChangeInformation toAddTo)
        {
            bool wasAdded = false;

            lock (LockObject)
            {
                bool wasIgnored = TryIgnoreFileChange(fileName);
                if (!wasIgnored)
                {
                    toAddTo.Add(fileName);
                    if (SortDelegate != null)
                    {
                        toAddTo.Sort(SortDelegate);
                    }
                    wasAdded = true;
                }
                mLastModification = DateTime.Now;
            }
            return wasAdded;
        }

        bool TryIgnoreFileChange(string fileName)
        {
            fileName = fileName.ToLower();
            int changesToIgnore = 0;

            fileName = FileManager.Standardize(fileName, null, false);

            if (mChangesToIgnore.ContainsKey(fileName))
            {
                changesToIgnore = mChangesToIgnore[fileName];

                mChangesToIgnore[fileName] = System.Math.Max(0, changesToIgnore - 1);
            }

            return changesToIgnore > 0;
        }

        public void IgnoreNextChangeOn(string fileName)
        {
            lock (LockObject)
            {

                if (FileManager.IsRelative(fileName))
                {
                    throw new Exception("File name should be absolute");
                }
                string standardized = FileManager.Standardize(fileName, null, false).ToLower();
                if (mChangesToIgnore.ContainsKey(standardized))
                {
                    mChangesToIgnore[standardized] = 1 + mChangesToIgnore[standardized];
                }
                else
                {
                    mChangesToIgnore[standardized] = 1;
                }
            }
        }

        public void Clear()
        {
            mDeletedFiles.Clear();
            mChangedFiles.Clear();
        }
    }
}