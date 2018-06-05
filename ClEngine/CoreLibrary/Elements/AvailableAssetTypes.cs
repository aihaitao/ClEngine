using System.Collections.Generic;
using System.Linq;

namespace ClEngine.CoreLibrary.Elements
{
    public class AvailableAssetTypes
    {
        public List<string> AdditionalExtensionsToTreatAsAssets
        {
            get;
            private set;
        }

        static AvailableAssetTypes mSelf = new AvailableAssetTypes();
        public static AvailableAssetTypes Self => mSelf;

        List<AssetTypeInfo> mCoreAssetTypes = new List<AssetTypeInfo>();
        List<AssetTypeInfo> mCustomAssetTypes = new List<AssetTypeInfo>();

        public IEnumerable<AssetTypeInfo> AllAssetTypes => mCoreAssetTypes.Concat(mCustomAssetTypes).ToList();
    }
}