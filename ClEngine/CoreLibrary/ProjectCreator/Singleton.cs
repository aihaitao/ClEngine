namespace ClEngine.Core.ProjectCreator
{
    public class Singleton<T> where T : new()
    {
        internal static T mSelf;

        public static T Self
        {
            get
            {
                if (mSelf == null)
                    mSelf = new T();

                return mSelf;
            }
        }
    }
}