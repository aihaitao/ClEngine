using System;
using ClEngine.CoreLibrary.Map;

namespace ClEngine.CoreLibrary.Asset
{
    public sealed class MapResolver : AssetResolver
    {
        public string MapName { get; set; }
        public MapResolver() : base("Map")
        {
            UseBundle = false;
        }

        public override string Extension { get; }
        public override string WatcherExtension { get; }

        /// <summary>
        /// 编译地图 编译顺序
        /// 移动原始文件至资源目录
        /// </summary>
        /// <param name="path"></param>
        public override void Compiler(string path)
        {
            if (string.IsNullOrWhiteSpace(MapName))
                Logger.Logger.Error("不允许地图名为空");

            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException($"编译资源路径不能为空:{nameof(path)}");
            
            MoveAsset(ref path);
            AddMapManage(path);
        }

        public void AddMapManage(string path)
        {
            MapManage.GetInstance().AddMap(MapName, path);
        }
    }
}