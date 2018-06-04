using System.Windows;
using System.Windows.Controls;
using ClEngine.CoreLibrary;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace ClEngine.View
{
    /// <summary>
    /// ElementViewWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ElementViewWindow : UserControl
    {
        public ElementViewWindow()
        {
            InitializeComponent();

            InitContextMenu();
        }

        private void InitContextMenu()
        {
            var contextMenu = new ContextMenu();

            var screenItem = new MenuItem
            {
                Header = Properties.Resources.AddScreen
            };
            screenItem.Click += ScreenItemOnClick;
            contextMenu.Items.Add(screenItem);

            var fileItem = new MenuItem
            {
                Header = Properties.Resources.AddFile
            };
            contextMenu.Items.Add(fileItem);

            var newFileItem = new MenuItem
            {
                Header = Properties.Resources.NewFile
            };
            fileItem.Items.Add(newFileItem);

            var existFileItem = new MenuItem
            {
                Header = Properties.Resources.ExistFile
            };
            fileItem.Items.Add(existFileItem);

            var splitter = new Separator();
            contextMenu.Items.Add(splitter);

            var setStartupScreenItem = new MenuItem
            {
                Header = Properties.Resources.SetStartupScreen
            };
            contextMenu.Items.Add(setStartupScreenItem);

            var addObject = new MenuItem
            {
                Header = Properties.Resources.AddObject
            };
            contextMenu.Items.Add(addObject);

            ElementTreeView.ContextMenu = contextMenu;
        }

        private void ScreenItemOnClick(object sender, RoutedEventArgs e)
        {
            if (ProjectManager.ProjectSave == null)
                MessageBox.Show(Properties.Resources.NeedHasProjectFirst);
            else
            {
                if (ProjectManager.StatusCheck() == ProjectManager.CheckResult.Passed)
                {
                    var tiw = new TextInputWindow();
                }
            }
        }
    }
}
