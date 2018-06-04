using System.Reflection;
using FlatRedBall.IO;

namespace ClEngine.CoreLibrary.IO
{
    public class ProjectLoader
    {
        public static string GetExeLocation()
        {
            return FileManager.Standardize(Assembly.GetAssembly(typeof(MainWindow)).Location.ToLowerInvariant());
        }
    }
}