using System;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Input;
using ClEngine.Core;
using DirectxGame;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace ClEngine.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private bool _isLoadProject;
        public bool IsLoadProject
        {
            get => _isLoadProject;
            set
            {
                _isLoadProject = value;
                RaisePropertyChanged(() => IsLoadProject);
                RaisePropertyChanged(() => IsGameRun);
            }
        }
        public ICommand SaveScriptCommand { get; set; }
        public ICommand RunGameCommand { get; set; }
        public ICommand ClearLogCommand { get; set; }

        private bool _isClearLog;
        public bool IsClearLog
        {
            get => _isClearLog;
            set
            {
                _isClearLog = value;
                RaisePropertyChanged(() => IsClearLog);
            }
        }

        private string _projectPosition;

        public string ProjectPosition
        {
            get => _projectPosition;
            set
            {
                _projectPosition = value;

                RaisePropertyChanged(() => ProjectPosition);
            }
        }

        private bool _isGameRun;
        public bool IsGameRun
        {
            get => IsLoadProject && !_isGameRun;
            set
            {
                _isGameRun = value;
                RaisePropertyChanged(()=>IsGameRun);
            }
        }


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            IsLoadProject = false;
            IsGameRun = false;

            IsClearLog = MessageCache.Messages.Count > 0;
            MessageCache.Messages.CollectionChanged += MessagesOnCollectionChanged;

            SaveScriptCommand = new RelayCommand(SaveScriptExecute);
            RunGameCommand = new RelayCommand(RunGameExecute);
            ClearLogCommand = new RelayCommand(ClearLogExecute);
        }

        private void ClearLogExecute()
        {
            MessageCache.Messages.Clear();
        }

        private void RunGameExecute()
        {
            if (!IsLoadProject)
                return;

            ClearLogExecute();
            SaveScriptExecute();
            IsGameRun = true;

            Environment.CurrentDirectory = ProjectPosition;

            using (var game = new Game1())
            {
                game.Exiting += (sender, args) =>
                {
                    IsGameRun = false;
                    game.IsMouseVisible = true;
                };
                game.Run();
            }

            Environment.CurrentDirectory = EditorRecord.EditorEnvironment;
        }

        private void MessagesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IsClearLog = MessageCache.Messages.Count > 0;
        }

        private void SaveScriptExecute()
        {
            Messenger.Default.Send(string.Empty, "SaveScript");
        }
    }
}