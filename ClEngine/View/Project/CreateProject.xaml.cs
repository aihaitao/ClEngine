using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using ClEngine.Model;
using GalaSoft.MvvmLight.Messaging;
using SelectionMode = System.Windows.Controls.SelectionMode;

namespace ClEngine
{
    /// <summary>
    /// CreateProject.xaml 的交互逻辑
    /// </summary>
    public partial class CreateProject : Window
    {
        public CreateProject()
        {
            InitializeComponent();
            ListBox.SelectionMode = SelectionMode.Single;
        }

        /// <summary>
        /// 列表框选中
        /// </summary>
        private void TreeViewItem_OnSelected(object sender, RoutedEventArgs e)
        {
            ListBox.Items.Clear();

            if (e.OriginalSource is TreeViewItem treeViewItem)
            {
                var headerContentControl = treeViewItem.Header as HeaderedContentControl;
                var result = headerContentControl?.Header.ToString().Contains("开始模板");
                if (result.HasValue && (bool)result)
                {
                    var textBlock = new TextBlock {Text = "空白模板"};
                    ListBox.Items.Add(textBlock);
                }
            }
            
            if (ListBox.Items.Count > 0)
                ListBox.SelectedItem = ListBox.Items[0];
        }
        
        /// <summary>
        /// 浏览文件
        /// </summary>
        private void Browser_OnClick(object sender, RoutedEventArgs e)
        {
            var openfileDialog = new FolderBrowserDialog();
            var result = openfileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ProjectPosition.Text = openfileDialog.SelectedPath;
            }
        }

        /// <summary>
        /// 创建工程
        /// </summary>
        private void CreateProject_OnClick(object sender, RoutedEventArgs e)
        {
            if (ListBox.SelectedItem == null || string.IsNullOrEmpty(ProjectName.Text) ||
                string.IsNullOrEmpty(ProjectPosition.Text))
            {
                var model = new LogModel
                {
                    Message = "请检查是否未选择模板或工程名为空或工程路径为空!",
                    LogLevel = LogLevel.Error
                };
                Messenger.Default.Send(model, "Log");
                return;
            }

            var gameRuntime = Path.Combine(Environment.CurrentDirectory, "runtime", "game");
            var files = Directory.GetFiles(gameRuntime, "*.*",SearchOption.AllDirectories);

            for (int i = files.Length - 1; i >= 0; i--)
            {
                var dirName = Path.GetDirectoryName(files[i]);
                if (dirName != null && !dirName.Equals(gameRuntime))
                {
                    var result = dirName.Replace(string.Concat(gameRuntime, "\\"), "");
                    var targetName = Path.Combine(ProjectPosition.Text, result);
                    if (!Directory.Exists(targetName))
                        Directory.CreateDirectory(targetName);

                    File.Copy(files[i],
                        Path.Combine(ProjectPosition.Text, result,
                            Path.GetFileName(files[i]) ?? throw new InvalidOperationException()), true);
                }
                else
                {
                    if (!Directory.Exists(ProjectPosition.Text))
                        Directory.CreateDirectory(ProjectPosition.Text);

                    File.Copy(files[i],
                        Path.Combine(ProjectPosition.Text, 
                            Path.GetFileName(files[i]) ?? throw new InvalidOperationException()), true);
                }


            }

            var projectModel = new ProjectModel
            {
                Position = ProjectPosition.Text,
                Name = ProjectName.Text
            };
            Messenger.Default.Send(projectModel,"LoadProject");

            DialogResult = true;
        }
    }
}
