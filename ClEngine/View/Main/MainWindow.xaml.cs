using System;
using System.Windows;
using System.Windows.Input;
using ClEngine.CoreLibrary.Logger;
using ClEngine.Model;
using ClEngine.View.Messages;
using ClEngine.ViewModel;
using Exceptionless;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using Application = System.Windows.Application;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

// ReSharper disable once CheckNamespace
namespace ClEngine
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly MessageProperty _property = new MessageProperty();


        public MainWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<LogModel>(this, "Log", Log, true);

            //LogBlock.ItemsSource = MessageCache.Messages;

            //ScriptLayout.IsVisible = false;

            Closed += (sender, args) =>
            {
                Messenger.Default.Unregister(this);
                ViewModelLocator.Cleanup();
                //Environment.Exit(0);
                Application.Current.Shutdown();
            };
            //LogBlock.MouseDoubleClick += LogBlockOnMouseDoubleClick;
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
            
            //_property.PropertyGrid.SelectedObject = LogBlock.SelectedItem;
            _property.PropertyGrid.ExpandAllProperties();
        }
        
        /// <summary>
        /// 创建工程
        /// </summary>
        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var createProject = CreateProjectWindow.GetInstance();
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
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = @"ClProject(*.cl)|*.cl",
            };
            var result = openFileDialog.ShowDialog();
        }

        public void Invoke(Action action)
        {
            Dispatcher.Invoke(action);
        }
    }
}
