using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using ClEngine.Core;
using ClEngine.CoreLibrary.Logger;
using ClEngine.Model;
using ClEngine.View.Messages;
using ClEngine.ViewModel;
using Exceptionless;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using Newtonsoft.Json;
using Application = System.Windows.Application;
using Size = System.Windows.Size;

// ReSharper disable once CheckNamespace
namespace ClEngine
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private ProjectInfo ProjectInfo { get; set; }
        private readonly MessageProperty _property = new MessageProperty();


        public MainWindow()
	    {
            InitializeComponent();
            Messenger.Default.Register<LogModel>(this, "Log", Log, true);
            Messenger.Default.Register<ProjectModel>(this, "LoadProject", LoadProject, true);

            LogBlock.ItemsSource = MessageCache.Messages;
            
	        MapLayout.IsVisible = false;
	        UiLayout.IsVisible = false;
	        ScriptLayout.IsVisible = false;
	        ParticleLayout.IsVisible = false;

            Closed += (sender, args) =>
            {
                Messenger.Default.Unregister(this);
                ViewModelLocator.Cleanup();
                Environment.Exit(0);
                //Application.Current.Shutdown();   // BUG: 进程退出后会导致 GraphicsDevice.Directx发生_swapChain错误.等待MonoGame.Framework.WpfInterop作者解决
            };
            LogBlock.MouseDoubleClick += LogBlockOnMouseDoubleClick;
	    }

        private void LogBlockOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_property.MessagePropertyIsShow == false)
            {
                _property.Show();
                _property.MessagePropertyIsShow = true;
            }
            else
            {
                _property.Activate();
            }
            
            _property.PropertyGrid.SelectedObject = LogBlock.SelectedItem;
            _property.PropertyGrid.ExpandAllGridItems();
        }
        
        /// <summary>
        /// 创建工程
        /// </summary>
        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var createProject = new CreateProject();
            createProject.ShowDialog();
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        public void Log(LogModel model)
        {
            switch (model.Message)
            {
                case null:
                    return;
                case string s:
                    switch (model.LogLevel)
                    {
                        case LogLevel.Log:
                            break;
                        case LogLevel.Error:
                            ExceptionlessClient.Default.SubmitLog(s, Exceptionless.Logging.LogLevel.Error);
                            break;
                        case LogLevel.Warn:
                            ExceptionlessClient.Default.SubmitLog(s, Exceptionless.Logging.LogLevel.Warn);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    
                    break;
            }
        }

        /// <summary>
        /// 打开工程
        /// </summary>
        private void OpenProject_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                AutoUpgradeEnabled = true,
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = @"ClProject(*.cl)|*.cl",
            };
            var result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                LoadProject(openFileDialog.FileName);
            }
        }

        private void LoadProject(string position)
        {
            var model = new ProjectModel {Position = position};

            LoadProject(model);
        }

        public void LoadProject(ProjectModel model)
        {
            var serializer = new JsonSerializer();
            if (!string.IsNullOrEmpty(model.Name))
            {
                ProjectInfo = new ProjectInfo {ProjectName = model.Name};
                ((MainViewModel)DataContext).ProjectPosition = model.Position;

                using (var streamWriter = new StreamWriter(Path.Combine(model.Position, string.Concat(model.Name, ".cl"))))
                {
                    using (var writer = new JsonTextWriter(streamWriter))
                    {
                        serializer.Formatting = Formatting.Indented;
                        serializer.Serialize(writer, ProjectInfo);
                    }
                }
            }
            else
            {
                ((MainViewModel)DataContext).ProjectPosition = Path.GetDirectoryName(model.Position);
                using (var streamReader = new StreamReader(model.Position))
                {
                    using (var reader = new JsonTextReader(streamReader))
                    {
                        serializer.Formatting = Formatting.Indented;
                        ProjectInfo = serializer.Deserialize<ProjectInfo>(reader);
                    }
                }
            }
            
            MapLayout.IsVisible = true;
            UiLayout.IsVisible = true;
            ScriptLayout.IsVisible = true;
            ParticleLayout.IsVisible = true;

            ((MainViewModel) DataContext).IsLoadProject = true;

            var size = new Size(1920, 1080);
            Messenger.Default.Send(size, "LoadMap");
            Messenger.Default.Send("", "LoadUiConfig");
			//MapDraw.Instance.SetContentRoot();
			//MapEditor.MapEditorViewModel.LoadMapList();
        }
    }
}
