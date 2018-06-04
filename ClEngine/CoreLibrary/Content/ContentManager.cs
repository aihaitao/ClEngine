using System;
using System.Collections.Generic;
using System.Threading;
using FlatRedBall.IO;

namespace ClEngine.CoreLibrary.Content
{
    public class ContentManager : Microsoft.Xna.Framework.Content.ContentManager
    {
        internal Dictionary<string, object> mAssets;
        internal string mName;
        public static bool LoadFromGlobalIfExists = true;

        internal Dictionary<string, IDisposable> mDisposableDictionary = new Dictionary<string, IDisposable>();
        internal Dictionary<string, object> mNonDisposableDictionary = new Dictionary<string, object>();

        public List<ManualResetEvent> ManualResetEventList
        {
            get;
            private set;
        }

#if DEBUG
        public static bool ThrowExceptionOnGlobalContentLoadedInNonGlobal
        {
            get;
            set;
        }
#endif

        public ContentManager(string name, IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            mName = name;
            mAssets = new Dictionary<string, object>();
            ManualResetEventList = new List<ManualResetEvent>();
        }

        public ContentManager(string name, IServiceProvider serviceProvider, string rootDictionary)
            : base(serviceProvider, rootDictionary)
        {
            mName = name;
            mAssets = new Dictionary<string, object>();
            ManualResetEventList = new List<ManualResetEvent>();
        }

        public bool IsAssetLoadedByName<T>(string assetName)
        {
            if (FileManager.IsRelative(assetName))
            {
                assetName = FileManager.MakeAbsolute(assetName);
            }

            assetName = FileManager.Standardize(assetName);

            var combinedName = assetName + typeof(T).Name;

            if (mDisposableDictionary.ContainsKey(combinedName))
            {
                return true;
            }

            return mNonDisposableDictionary.ContainsKey(combinedName) || mAssets.ContainsKey(combinedName);
        }

        public T GetNonDisposable<T>(string objectName)
        {
            if (FileManager.IsRelative(objectName))
            {
                objectName = FileManager.MakeAbsolute(objectName);
            }

            objectName += typeof(T).Name;

            return (T)mNonDisposableDictionary[objectName];
        }
    }
}