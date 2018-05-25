using System.Globalization;
using System.Threading;
using System.Windows;
using Exceptionless;

namespace ClEngine.View.Main
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
	    public App()
	    {
		    ExceptionlessClient.Default.Register();
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(CultureInfo.InstalledUICulture.Name);
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
        }
        
    }
}
