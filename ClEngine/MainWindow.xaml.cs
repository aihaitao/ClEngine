using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using ClEngine.Model;
using ClEngine.View.Map;
using ClEngine.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;

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
        private Queue<string> Messages { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<LogModel>(this, "Log", Log);
            Messenger.Default.Register<ProjectModel>(this, "LoadProject", LoadProject);
            DataContext = new MainViewModel();
            Messages = new Queue<string>();

            TabControl.Visibility = Visibility.Hidden;
            IsLoadProject = false;
            var backgroundworker = new BackgroundWorker();
            backgroundworker.DoWork += (sender, args) => BeginOutputLog();
            backgroundworker.RunWorkerAsync();

            SaveBtn.Click += (sender, args) => Messenger.Default.Send("", "SaveScript");
        }

        private void BeginOutputLog()
        {
            while (true)
            {
                if (Messages.Count > 0)
                {
                    LogBlock.Dispatcher.Invoke(new Action(() => LogBlock.Text += Messages.Dequeue()));
                }
            }
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
                Log("找不到所需运行库!");
                return;
            }

            var processInfo = new ProcessStartInfo(fileName)
            {
                WorkingDirectory = ProjectPosition,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            };
            var process = Process.Start(processInfo);
            if (process != null)
            {
                process.OutputDataReceived += ProcessOnOutputDataReceived;
                process.ErrorDataReceived += ProcessOnErrorDataReceived;
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
            }
        }

        private void ProcessOnErrorDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            var model = new LogModel
            {
                LogLevel = LogLevel.Log,
                Message = dataReceivedEventArgs.Data
            };
            Log(model);
        }

        private void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            var model = new LogModel
            {
                LogLevel = LogLevel.Log,
                Message = dataReceivedEventArgs.Data
            };
            Log(model);
        }

        public void Log(string content, LogLevel logLevel = LogLevel.Log)
        {
            var model = new LogModel
            {
                LogLevel = logLevel,
                Message = content
            };

            Log(model);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        public void Log(LogModel model)
        {
	        if (string.IsNullOrWhiteSpace(model.Message))
		        return;

			var preview = string.Empty;

            switch (model.LogLevel)
            {
                case LogLevel.Log:
                    preview = "[记录]: ";
                    break;
                case LogLevel.Warn:
                    preview = "[警告]: ";
                    break;
                case LogLevel.Error:
                    preview = "[错误]: ";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
			
            Messages.Enqueue(preview + model.Message + Environment.NewLine);
        }

        /// <summary>
        /// 清除日志
        /// </summary>
        private void ClearLog(object sender, RoutedEventArgs e)
        {
            LogBlock.Text = string.Empty;
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
                Filter = @"CL工程文件(*.cl)|*.cl",
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
        }

	    private const string ParticleName = "ProjectMercury.EffectEditor.exe";

		private void ParticleEditor_OnClick(object sender, RoutedEventArgs e)
		{
			var runtimeParticle = Path.Combine(Environment.CurrentDirectory, "runtime", "particle");
			var runtimeParticleExecute = Path.Combine(runtimeParticle, ParticleName);
		    if (File.Exists(runtimeParticleExecute))
		    {
			    var processInfo = new ProcessStartInfo(runtimeParticleExecute) {WorkingDirectory = runtimeParticle};

			    Process.Start(processInfo);
		    }
	    }
    }
}
