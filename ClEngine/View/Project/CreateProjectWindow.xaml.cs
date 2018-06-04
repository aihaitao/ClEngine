using System.Windows;
using System.Windows.Forms;
using ClEngine.CoreLibrary.ProjectCreator;

namespace ClEngine
{
    /// <summary>
    /// CreateProject.xaml 的交互逻辑
    /// </summary>
    public partial class CreateProjectWindow : Window
    {
        private static CreateProjectWindow Instance { get; set; }
        private CreateProjectWindow()
        {
            InitializeComponent();

            DataLoader.LoadAvaliableProjectFromCsv();
        }

        public static CreateProjectWindow GetInstance()
        {
            return Instance ?? (Instance = new CreateProjectWindow());
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
