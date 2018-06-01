using System.Collections.Generic;
using ClEngine.CoreLibrary.SaveClasses;

namespace ClEngine.CoreLibrary.Manager
{
    public class UnreferencedFilesManager
    {
        #region Fields

        static List<ProjectSpecificFile> mLastAddedUnreferencedFiles = new List<ProjectSpecificFile>();
        static List<string> mListBeforeAddition = new List<string>();
        static List<ProjectSpecificFile> mUnreferencedFiles = new List<ProjectSpecificFile>();

        public bool mHasHadFailure = false;


        static UnreferencedFilesManager mSelf;



        #endregion

        #region Properties

        public static UnreferencedFilesManager Self
        {
            get
            {
                if (mSelf == null)
                {
                    mSelf = new UnreferencedFilesManager();
                }
                return mSelf;
            }
        }

        public static List<ProjectSpecificFile> LastAddedUnreferencedFiles
        {
            get
            {
                // Makes this getter thread-safe.
                List<ProjectSpecificFile> toReturn = new List<ProjectSpecificFile>();

                // This caused dead locks and I'm not sure we need it
                //lock (mLastAddedUnreferencedFiles)
                {
                    toReturn.AddRange(mLastAddedUnreferencedFiles);
                }

                return toReturn;
            }
        }

        public List<ProjectSpecificFile> UnreferencedFiles
        {
            get
            {
                return mUnreferencedFiles;
            }
        }

        bool mIsRefreshRequested;
        public bool IsRefreshRequested
        {
            get { return mIsRefreshRequested; }
            set
            {
                mIsRefreshRequested = value;
            }
        }

        #endregion
    }
}