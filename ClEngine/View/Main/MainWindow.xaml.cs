﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using ClEngine.Core;
using ClEngine.CoreLibrary.Logger;
using ClEngine.CoreLibrary.Map;
using ClEngine.Model;
using ClEngine.View.Messages;
using Exceptionless;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using Application = System.Windows.Application;

namespace ClEngine
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        private bool IsLoadProject { get; set; }
        public static string ProjectPosition { get; set; }
        private ProjectInfo ProjectInfo { get; set; }
        private readonly MessageProperty _property = new MessageProperty();

	    public MainWindow()
	    {
            InitializeComponent();
            Messenger.Default.Register<LogModel>(this, "Log", Log);
            Messenger.Default.Register<ProjectModel>(this, "LoadProject", LoadProject);

            LogBlock.ItemsSource = MessageCache.Messages;

            TabControl.Visibility = Visibility.Hidden;
            IsLoadProject = false;

	        Closed += (sender, args) => Application.Current.Shutdown();
            SaveBtn.Click += (sender, args) => Messenger.Default.Send("", "SaveScript");
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
        /// 运行游戏
        /// </summary>
        private void RunGame(object sender, RoutedEventArgs e)
        {
            if (!IsLoadProject)
            {
                return;
            }

            var fileName = Path.Combine(ProjectPosition, "ClGame.exe");
            if (!File.Exists(fileName))
            {
                Logger.Log("找不到所需运行库!");
                return;
            }
            
            Environment.CurrentDirectory = ProjectPosition;

            ClGame.MainWindow mainWindow = new ClGame.MainWindow();
            mainWindow.ShowDialog();

            Environment.CurrentDirectory = EditorRecord.EditorEnvironment;

            //var processInfo = new ProcessStartInfo(fileName)
            //{
            //    WorkingDirectory = ProjectPosition,
            //    RedirectStandardInput = true,
            //    RedirectStandardError = true,
            //    RedirectStandardOutput = true,
            //    UseShellExecute = false,
            //};
            //var process = Process.Start(processInfo);
            //if (process != null)
            //{
            //    process.OutputDataReceived += ProcessOnOutputDataReceived;
            //    process.ErrorDataReceived += (o, args) => Logger.Log(args.Data);
            //    process.BeginErrorReadLine();
            //    process.BeginOutputReadLine();
            //}
        }

        private void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Logger.Log(e.Data);
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
        /// 清除日志
        /// </summary>
        private void ClearLog(object sender, RoutedEventArgs e)
        {
            MessageCache.Messages.Clear();
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
                ProjectPosition = model.Position;

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
                ProjectPosition = Path.GetDirectoryName(model.Position);
                using (var streamReader = new StreamReader(model.Position))
                {
                    using (var reader = new JsonTextReader(streamReader))
                    {
                        serializer.Formatting = Formatting.Indented;
                        ProjectInfo = serializer.Deserialize<ProjectInfo>(reader);
                    }
                }
            }

            TabControl.Visibility = Visibility.Visible;
            IsLoadProject = true;

            var size = new Size(1920, 1080);
            Messenger.Default.Send(size, "LoadMap");
            Messenger.Default.Send("", "LoadUiConfig");
			MapDraw.Instance.SetContentRoot();
			MapEditor.MapEditorViewModel.LoadMapList();
        }
    }
}
