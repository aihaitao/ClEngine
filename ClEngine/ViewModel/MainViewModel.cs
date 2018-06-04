using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using ClEngine.Core;
using ClEngine.CoreLibrary;
using ClEngine.CoreLibrary.IO;
using ClEngine.CoreLibrary.SaveClasses;
using ClEngine.Properties;
using CommonServiceLocator;
using FlatRedBall.Glue.VSHelpers;
using FlatRedBall.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

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

        public bool _isFormal;

        public bool IsFormal
        {
            get => _isFormal;
            set
            {
                _isFormal = value;
                RaisePropertyChanged(() => IsFormal);
            }
        }

        public ICommand SaveScriptCommand { get; set; }
        public ICommand RunGameCommand { get; set; }
        public ICommand ClearLogCommand { get; set; }
        public ICommand OpenProjectCommand { get; set; }

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
            IsFormal = false;

            IsClearLog = MessageCache.Messages.Count > 0;
            MessageCache.Messages.CollectionChanged += MessagesOnCollectionChanged;

            SaveScriptCommand = new RelayCommand(SaveScriptExecute);
            RunGameCommand = new RelayCommand(RunGameExecute);
            ClearLogCommand = new RelayCommand(ClearLogExecute);
            OpenProjectCommand = new RelayCommand(OpenProject);
        }

        private void OpenProject()
        {
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "C:\\",
                Filter = $"{Resources.ProjectFiles} (*.vcproj;*.csproj;*.sln)|*.vcproj;*.csproj;*.sln;",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var projectFileName = openFileDialog.FileName;

                if (FileManager.GetExtension(projectFileName) == "sln")
                {
                    var solution = VSSolution.FromFile(projectFileName);

                    var solutionName = projectFileName;

                    projectFileName = solution.ReferencedProjects.FirstOrDefault(item =>
                    {
                        var isRegularProject = FileManager.GetExtension(item) == "csproj" ||
                                               FileManager.GetExtension(item) == "vsproj";

                        var hasSameName =
                            FileManager.RemovePath(FileManager.RemoveExtension(solutionName)).ToLowerInvariant() ==
                            FileManager.RemovePath(FileManager.RemoveExtension(item)).ToLowerInvariant();

                        return isRegularProject && hasSameName;
                    });

                    projectFileName = FileManager.GetDirectory(solutionName) + projectFileName;
                }

                ServiceLocator.Current.GetInstance<ProjectViewModel>().LoadProject(projectFileName);

                SaveSettings();
            }
        }

        private void SaveSettings()
        {
            var save = ProjectManager.SettingsSave;

            string lastFileName = null;

            if (ProjectManager.ProjectBase != null)
                lastFileName = ProjectManager.ProjectBase.FullFileName;

            save.LastProjectFile = lastFileName;

            var exeFileName = ProjectLoader.GetExeLocation();
            var foundItem = save.LocationSpecificLastProjectFiles.FirstOrDefault(item => item.FileName == exeFileName);

            var alreadyIsListed = foundItem != null;

            if (!alreadyIsListed)
            {
                foundItem = new ProjectFileFilePair();
                save.LocationSpecificLastProjectFiles.Add(foundItem);
            }

            foundItem.FileName = exeFileName;
            foundItem.GameProjectFileName = lastFileName;

            save.Save();
        }

        private void ClearLogExecute()
        {
            MessageCache.Messages.Clear();
        }

        private void RunGameExecute()
        {
        }

        private void MessagesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IsClearLog = MessageCache.Messages.Count > 0;
        }

        private void SaveScriptExecute()
        {
            ScriptWindow.SaveScript();
        }
    }
}