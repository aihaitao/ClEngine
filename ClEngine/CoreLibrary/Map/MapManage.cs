using System;
using System.Collections.Generic;

namespace ClEngine.CoreLibrary.Map
{
    [System.Serializable]
    public class MapManage
    {
        public const string ManageFile = "manage.map";
        private static MapManage Instance { get; set; }
        
        private readonly Dictionary<string, string> _mapDictionary;

        private MapManage()
        {
            _mapDictionary = new Dictionary<string, string>();
        }

        public static MapManage GetInstance()
        {
            return Instance ?? (Instance = new MapManage());
        }

        public void AddMap(string mapName, string mapPath)
        {
            _mapDictionary.Add(mapName, mapPath);
        }
    }
}