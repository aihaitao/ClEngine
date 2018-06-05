using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ClEngine.CoreLibrary;
using ClEngine.CoreLibrary.Controls;
using ClEngine.CoreLibrary.FormHelpers;
using ClEngine.CoreLibrary.Plugins.ExportedImplementations;
using ClEngine.ViewModel;
using CommonServiceLocator;
using FlatRedBall.IO;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace ClEngine.View
{
    /// <summary>
    /// ElementViewWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ElementViewWindow : UserControl
    {
        internal static List<string> mDirectoriesToIgnore = new List<string>();

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

        internal static void AddDirectoryNodes()
        {
            AddDirectoryNodes(FileManager.RelativeDirectory + "Entities/", ElementViewModel.mEntityNode);

            var contentDirectory = FileManager.RelativeDirectory;

            if (ProjectManager.ContentProject != null)
            {
                contentDirectory = ProjectManager.ContentProject.GetAbsoluteContentFolder();
            }

            AddDirectoryNodes(contentDirectory + "GlobalContent/", ElementViewModel.mGlobalContentNode);
        }

        internal static void AddDirectoryNodes(string parentDirectory, TreeViewItem parentTreeItem)
        {
            if (Directory.Exists(parentDirectory))
            {
                var direcotories = Directory.GetDirectories(parentDirectory);

                foreach (var direcotory in direcotories)
                {
                    var relativePath = FileManager.MakeRelative(direcotory, parentDirectory);

                    var nameOfNewNode = relativePath;

                    if (relativePath.Contains('/'))
                    {
                        nameOfNewNode = relativePath.Substring(0, relativePath.IndexOf('/'));
                    }

                    if (!mDirectoriesToIgnore.Contains(nameOfNewNode))
                    {
                        var treeNode = GlueState.Self.Find.TreeNodeForDirectoryOrEntityNode(relativePath, parentTreeItem);

                        if (treeNode == null)
                        {
                            treeNode = new TreeViewItem
                            {
                                Header = FileManager.RemovePath(direcotory)
                            };
                            parentTreeItem.Items.Add(treeNode);
                        }
                        
                        AddDirectoryNodes(parentDirectory + relativePath + "/", treeNode);
                    }
                }

                for (var i = 0; i < direcotories.Length; i++)
                {
                    direcotories[i] = FileManager.Standardize(direcotories[i]).ToLower();

                    if (!direcotories[i].EndsWith("/") && !direcotories[i].EndsWith("\\"))
                    {
                        direcotories[i] = direcotories[i] + "/";
                    }
                }

                var isGlobalContent = parentTreeItem.Root().IsGlobalContentContainerNode();

                for (var i = parentTreeItem.Items.Count - 1; i > -1; i--)
                {
                    var treeNode = (TreeViewItem)parentTreeItem.Items[i];

                    if (treeNode.IsDirectoryNode())
                    {
                        var directory = ProjectManager.MakeAbsolute(treeNode.GetRelativePath(), isGlobalContent);

                        directory = FileManager.Standardize(directory.ToLower());

                        if (!direcotories.Contains(directory))
                        {
                            parentTreeItem.Items.RemoveAt(i);
                        }
                    }
                }
            }
        }
    }
}
