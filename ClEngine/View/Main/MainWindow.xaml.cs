using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using ClEngine.CoreLibrary;
using ClEngine.CoreLibrary.Manager;
using ClEngine.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Application = System.Windows.Application;

// ReSharper disable once CheckNamespace
namespace ClEngine
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Closed += (sender, args) =>
            {
                Messenger.Default.Unregister(this);
                ViewModelLocator.Cleanup();
                //Environment.Exit(0);
                Application.Current.Shutdown();
            };
        }
        
        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var createProject = CreateProjectWindow.GetInstance();
            createProject.ShowDialog();
        }

        public static void CloseProject(bool shouldSave, bool isExiting)
        {
            ProjectManager.WantsToClose = true;

            while (TaskManager.Self.AreAllAsyncTasksDone == false)
            {
                Thread.Sleep(50);
                
                System.Windows.Forms.Application.DoEvents();
            }

            if (shouldSave)
            {
                if (ProjectManager.ProjectBase != null &&
                    !string.IsNullOrEmpty(ProjectManager.ProjectBase.FullFileName))
                {

                }
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (FileNotFoundException fileNotFoundException)
            {
                if (fileNotFoundException.ToString().Contains("Microsoft.Xna.Framework.dll"))
                {
                    var message = Properties.Resources.CantLoadMayBeNotInstallXna;
                    MessageBox.Show(message);
                    Process.Start("https://www.microsoft.com/en-us/download/details.aspx?id=20914");
                    Close();
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
