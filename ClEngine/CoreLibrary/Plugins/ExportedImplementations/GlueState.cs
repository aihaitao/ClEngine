using ClEngine.CoreLibrary.Manager;

namespace ClEngine.CoreLibrary.Plugins.ExportedImplementations
{
    public class GlueState : IGlueState
    {
        static GlueState mSelf;
        public static GlueState Self
        {
            get
            {
                if (mSelf == null)
                {
                    mSelf = new GlueState();
                }
                return mSelf;
            }
        }

        public IFindManager Find
        {
            get;
            private set;
        }

        public string CurrentGlueProjectFileName => ProjectManager.ProjectBase?.FullFileName;
    }
}