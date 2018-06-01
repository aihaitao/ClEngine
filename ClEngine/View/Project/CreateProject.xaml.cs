using System.Windows;
using System.Windows.Forms;
using ClEngine.Core.ProjectCreator;

namespace ClEngine
{
    /// <summary>
    /// CreateProject.xaml 的交互逻辑
    /// </summary>
    public partial class CreateProject : Window
    {
        private static CreateProject Instance { get; set; }
        private CreateProject()
        {
            InitializeComponent();

            DataLoader.LoadAvaliableProjectFromCsv();
        }

        public static CreateProject GetInstance()
        {
            return Instance ?? (Instance = new CreateProject());
        }

        /// <summary>
        /// 浏览文件
        /// </summary>
        private void Browser_OnClick(object sender, RoutedEventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            var result = fbd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ProjectLocationTextBox.Text = fbd.SelectedPath;

                UserDifferentNamespaceCheckBoxChanged();
            }
        }

        private void UserDifferentNamespaceCheckBoxChanged()
        {
            DifferentNamespaceTextbox.Visibility = DifferentNamespaceCheckbox.IsChecked == true ? Visibility.Hidden : Visibility.Visible;
        }
    }
}
