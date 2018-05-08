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
	    }
    }
}
