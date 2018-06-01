using System;

namespace ClEngine.CoreLibrary.Task
{
    public class GlueTask
    {
        public Action Action
        {
            get;
            set;
        }

        public string DisplayInfo
        {
            get;
            set;
        }

        public string TaskType
        {
            get;
            set;
        }
    }
}