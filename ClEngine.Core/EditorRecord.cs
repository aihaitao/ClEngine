using System.Resources;
using Xceed.Wpf.AvalonDock.Properties;

namespace ClEngine.Core
{
    public static class EditorRecord
	{
	    public static dynamic GetTranslateName(this object obj)
	    {
	        if (obj is string name)
	        {
	            return BeginTranslateName(name);
	        }

	        return obj;
	    }

	    private static string BeginTranslateName(string name)
	    {
	        if (string.IsNullOrWhiteSpace(name))
	            return string.Empty;

	        var resourceManager = new ResourceManager("ClEngine.Core.Resources", typeof(Resources).Assembly);
	        var translateName = resourceManager.GetObject(name);
	        return translateName != null ? translateName.ToString() : name;
	    }
    }
}