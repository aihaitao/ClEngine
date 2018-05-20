namespace ClEngine.CoreLibrary.Asset
{
    public sealed class MapResolver : AssetResolver
    {
        public const string MapManage = "manage.map";

        public MapResolver() : base("Map")
        {
            UseBundle = false;
        }

        public override string Extension { get; }
        public override string WatcherExtension { get; }
    }
}