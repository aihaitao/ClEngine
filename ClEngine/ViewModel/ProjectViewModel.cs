using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using ClEngine.Core.ProjectCreator;
using ClEngine.Properties;
using ClEngine.View.Project;
using FlatRedBall.IO;
using FRBDKUpdater;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Xceed.Wpf.Toolkit;

namespace ClEngine.ViewModel
{
    public class TemplateCategoryViewModel
    {
        public string Name { get; set; }
    }

    public class TemplateViewModel
    {
        public string Name => BackingData.FriendlyName;

        public PlatformProjectInfo BackingData { get; set; }
    }
    
    public class ProjectViewModel : ViewModelBase
    {
        internal char[] InvalidNamespaceCharacters = {
            '~', '`', '!', '@', '#', '$', '%', '^', '&', '*',
            '(', ')', '-', '=', '+', ';', '\'', ':', '"', '<',
            ',', '>', '.', '/', '\\', '?', '[', '{', ']', '}',
            '|', 
            // Spaces are handled separately
            //    ' ' 
        };

        public bool OpenSlnFolderAfterCreation { get; set; }
        public string ProjectName { get; set; }
        public bool UseDifferentNamespace { get; set; }
        public string ProjectLocation { get; set; }
        public bool CreateProjectDirectory { get; set; }
        public string DifferentNamespace { get; set; }
        public bool CheckForNewVersions { get; set; }

        public PlatformProjectInfo ProjectType => SelectedTemplate?.BackingData;

        public ICommand MakeNewProjectCommand { get; set; }
        public ObservableCollection<TemplateCategoryViewModel> Categories => FilteredCategories;

        private TemplateCategoryViewModel _selectedCategory;

        public TemplateCategoryViewModel SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                RaisePropertyChanged(() => SelectedCategory);

                RefreshAvailableTemplates();
            }
        }

        public string CombineProjectDirectory
        {
            get
            {
                if (!ProjectLocation.EndsWith("\\") && !ProjectLocation.EndsWith("/"))
                    return ProjectLocation + "\\" + ProjectName;

                return ProjectLocation + ProjectName;
            }
        }

        public ObservableCollection<TemplateViewModel> AvailableTemplates { get; }

        private TemplateViewModel _selectedTemplate;
        public TemplateViewModel SelectedTemplate
        {
            get => _selectedTemplate;
            set
            {
                _selectedTemplate = value;
                RaisePropertyChanged(() => SelectedTemplate);
            }
        }

        internal TemplateCategoryViewModel StarterCategory;
        internal TemplateCategoryViewModel EmptyProjectsCategory;

        internal ObservableCollection<TemplateCategoryViewModel> AllCategories = new ObservableCollection<TemplateCategoryViewModel>();
        internal ObservableCollection<TemplateCategoryViewModel> FilteredCategories = new ObservableCollection<TemplateCategoryViewModel>();
        
        public ProjectViewModel()
        {
            AvailableTemplates = new ObservableCollection<TemplateViewModel>();
            
            StarterCategory = new TemplateCategoryViewModel
            {
                Name = Resources.StarterProjects
            };

            EmptyProjectsCategory = new TemplateCategoryViewModel
            {
                Name = Resources.EmptyProjects
            };

            AllCategories.Add(StarterCategory);
            AllCategories.Add(EmptyProjectsCategory);

            foreach (var templateCategoryViewModel in AllCategories)
            {
                FilteredCategories.Add(templateCategoryViewModel);
            }

            SelectedCategory = FilteredCategories[0];

            RefreshAvailableTemplates();

            MakeNewProjectCommand = new RelayCommand(HandleMakeNewProject);

            ProjectName = "NewProject";
            ProjectLocation = @"C:\NewProjects";
            DifferentNamespace = "NewProject";
            CreateProjectDirectory = true;
            CheckForNewVersions = true;
        }

        public bool MakeNewProject()
        {
            GetDefaultZipLocationAndStringToReplace(ProjectType, out var zipToUnpack, out var stringToReplace);
            var fileToDownload = GetFileToDownload();

            var succeeded = true;

            if (CommandLineManager.Self.OpenedBy != null && CommandLineManager.Self.OpenedBy.ToLower() == "glue")
            {
                var ppi = ProjectType;

                if (!ppi.SupportedInGlue)
                {
                    succeeded = false;
                    MessageBox.Show(Resources.NotSuppoertGlueManuallyCreator);
                }
            }

            var unpackDirectory = ProjectLocation;

            if (succeeded)
            {
                var isFileNameValid = GetIfFileNameIsValid(ref unpackDirectory);

                if (!isFileNameValid)
                    succeeded = false;
            }

            if (succeeded)
            {
                var hasUserCancelled = false;

                zipToUnpack = GetZipToUnpack(fileToDownload, ref hasUserCancelled, out var shouldTryDownloading);

                if (shouldTryDownloading)
                {
                    var downloadSucceeded = DownloadFileSync(zipToUnpack, fileToDownload);

                    if (!downloadSucceeded)
                        ShowErrorMessageBox(ref hasUserCancelled, ref zipToUnpack, Resources.DownloadError);
                }

                if (!hasUserCancelled)
                {
                    try
                    {
                        Directory.CreateDirectory(unpackDirectory);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        succeeded = false;
                        MessageBox.Show(
                            $"{Resources.NotPermissionCreate}\n\n{unpackDirectory}\n\n{Resources.RunAdminMode}");
                    }

                    if (succeeded)
                    {
                        if (!File.Exists(zipToUnpack))
                            MessageBox.Show($"{Resources.CantFindTemplate}:\n{zipToUnpack}");

                        succeeded = UnzipManager.UnzipFile(zipToUnpack, unpackDirectory);
                    }

                    if (succeeded)
                    {
                        RenameEverything(stringToReplace, unpackDirectory);
                        
                        GuidLogic.ReplaceGuids(unpackDirectory);

                        if (OpenSlnFolderAfterCreation)
                            Process.Start(unpackDirectory);

                        Console.Out.WriteLine(unpackDirectory);
                    }
                }
            }

            return succeeded;
        }

        private void RenameEverything(string stringToReplace, string unpackDirectory)
        {
            var newProjectName = ProjectName;

            if (stringToReplace != newProjectName)
            {
                RenameFile(unpackDirectory, stringToReplace, newProjectName);

                UpdateSolutionContents(unpackDirectory, stringToReplace, newProjectName);
            }

            var newNamespace = DifferentNamespace;

            if (stringToReplace != newNamespace)
            {
                UpdateNamespaces(unpackDirectory, stringToReplace, newNamespace);
            }
        }

        private void UpdateNamespaces(string unpackDirectory, string stringToReplace, string stringToReplaceWith)
        {
            var filesToFix = FileManager.GetAllFilesInDirectory(unpackDirectory, "cs");
            
            filesToFix.AddRange(FileManager.GetAllFilesInDirectory(unpackDirectory, "xaml"));
            filesToFix.AddRange(FileManager.GetAllFilesInDirectory(unpackDirectory, "aspx"));
            filesToFix.AddRange(FileManager.GetAllFilesInDirectory(unpackDirectory, "html"));
            filesToFix.AddRange(FileManager.GetAllFilesInDirectory(unpackDirectory, "user"));
            filesToFix.AddRange(FileManager.GetAllFilesInDirectory(unpackDirectory, "appxmanifest"));

            foreach (var fileName in filesToFix)
            {
                var contents = FileManager.FromFileText(fileName);

                contents = contents.Replace(stringToReplace, stringToReplaceWith);

                FileManager.SaveText(contents, fileName);
            }

            filesToFix.Clear();

            filesToFix = FileManager.GetAllFilesInDirectory(unpackDirectory, "csv");

            foreach (var fileName in filesToFix)
            {
                var contents = FileManager.FromFileText(fileName);
                contents = contents.Replace(stringToReplace, stringToReplaceWith);
                FileManager.SaveText(contents, fileName);
            }

            filesToFix.Clear();
        }

        private void UpdateSolutionContents(string unpackDirectory, string stringToReplace, string stringToReplaceWith)
        {
            var filesToFix = FileManager.GetAllFilesInDirectory(unpackDirectory, "sln");

            foreach (var fileName in filesToFix)
            {
                var contents = FileManager.FromFileText(fileName);

                contents = contents.Replace(stringToReplace, stringToReplaceWith);
                
                FileManager.SaveText(contents, fileName);
            }

            foreach (var fileName in filesToFix)
            {
                EncodeSlnFiles(fileName);
            }
        }

        internal void EncodeSlnFiles(string fileName)
        {
            var streamRead = new StreamReader(fileName);
            var fileContents = streamRead.ReadToEnd();
            streamRead.Close();
            var streamWriter = new StreamWriter(fileName, false, Encoding.UTF8);
            streamWriter.Write(fileContents);
            streamWriter.Close();
        }

        private void RenameFile(string unpackDirectory, string stringToReplace, string stringToReplaceWith)
        {
            var filesToReplace = FileManager.GetAllFilesInDirectory(unpackDirectory, "csproj");

            filesToReplace.AddRange(FileManager.GetAllFilesInDirectory(unpackDirectory, "sln"));

            filesToReplace.AddRange(FileManager.GetAllFilesInDirectory(unpackDirectory, "suo"));

            filesToReplace.AddRange(FileManager.GetAllFilesInDirectory(unpackDirectory, "html"));

            filesToReplace.AddRange(FileManager.GetAllFilesInDirectory(unpackDirectory, "aspx"));

            filesToReplace.AddRange(FileManager.GetAllFilesInDirectory(unpackDirectory, "apk"));

            filesToReplace.AddRange(FileManager.GetAllFilesInDirectory(unpackDirectory, "java"));

            filesToReplace.AddRange(FileManager.GetAllFilesInDirectory(unpackDirectory, "contentproj"));

            filesToReplace.AddRange(FileManager.GetAllFilesInDirectory(unpackDirectory, "glux"));

            filesToReplace.AddRange(FileManager.GetAllFilesInDirectory(unpackDirectory, "pfx"));

            foreach (var fileName in filesToReplace)
            {
                if (fileName.Contains(stringToReplace))
                {
                    var directory = FileManager.GetDirectory(fileName);
                    var fileWithoutPath = FileManager.RemovePath(fileName);

                    fileWithoutPath = fileWithoutPath.Replace(stringToReplace, stringToReplaceWith);

                    TryMultipleTimes(() => File.Move(fileName, directory + fileWithoutPath), 5);
                }
            }

            var shouldRepreat = true;

            var directoriesAlreadyRenamed = new List<string>();

            while (shouldRepreat)
            {
                var directories =
                    Directory.GetDirectories(unpackDirectory, stringToReplace + "*", SearchOption.AllDirectories);

                int i;

                for (i = 0; i < directories.Length; i++)
                {
                    var fileName = directories[i];

                    if (!directoriesAlreadyRenamed.Contains(fileName) && fileName.Contains(stringToReplace))
                    {
                        var lastIndexOfWhatToReplace = fileName.LastIndexOf(stringToReplace, StringComparison.Ordinal);
                        var beforeLastIndex = fileName.Substring(0, lastIndexOfWhatToReplace);
                        var after = fileName.Substring(lastIndexOfWhatToReplace,
                            fileName.Length - lastIndexOfWhatToReplace);
                        after = after.Replace(stringToReplace, stringToReplaceWith);

                        var targetDirectory = beforeLastIndex + after;

                        TryMultipleTimes(() => Directory.Move(fileName, targetDirectory), 5);

                        directoriesAlreadyRenamed.Add(targetDirectory);

                        break;
                    }
                }

                if (i >= directories.Length)
                    shouldRepreat = false;
            }
        }

        private void TryMultipleTimes(Action action, int numberOfTimesToTry)
        {
            const int msSleep = 200;

            var failureCount = 0;

            while (failureCount < numberOfTimesToTry)
            {
                try
                {
                    action();
                    break;
                }
                catch (Exception)
                {
                    failureCount++;
                    Thread.Sleep(msSleep);
                    if (failureCount >= numberOfTimesToTry)
                    {
                        throw;
                    }
                }
            }
        }

        private bool DownloadFileSync(string zipToUnpack, string fileToDownload)
        {
            var eee = EmbeddedExecutableExtractor.Self;

            eee.ExtractFile("FlatRedBall.Tools.dll");
            eee.ExtractFile("Ionic.Zip.dll");
            eee.ExtractFile("Ionic.Zlib.dll");
            var resultingLocation = eee.ExtractFile("FRBDKUpdater.exe");

            var urs = new UpdaterRuntimeSettings
            {
                FileToDownload = fileToDownload,
                FormTitle = $"{Resources.Downloading} {ProjectType.FriendlyName}"
            };

            if (string.IsNullOrEmpty(zipToUnpack))
                throw new Exception(Resources.ZipToUnpackNotNull);

            urs.LocationToSaveFile = zipToUnpack;

            var whereToSaveSettings = FileManager.UserApplicationDataForThisApplication +
                                      "DownloadInformation." + UpdaterRuntimeSettings.RuntimeSettingsExtension;
            urs.Save(whereToSaveSettings);

            var psi = new ProcessStartInfo
            {
                FileName = resultingLocation,
                Arguments = "\"" + whereToSaveSettings + "\""
            };


            var process = Process.Start(psi);
            while (process != null && !process.HasExited)
                Thread.Sleep(200);

            var succeeded = process != null && process.ExitCode == 0;

            return succeeded;
        }

        private string GetZipToUnpack(string fileToDownload, ref bool hasUserCancelled, out bool shouldTryDownloading)
        {
            var checkOnline = CheckForNewVersions;
            string zipToUnpack = null;
            shouldTryDownloading = false;
            if (!string.IsNullOrEmpty(fileToDownload))
                zipToUnpack = FileManager.UserApplicationDataForThisApplication +
                              FileManager.RemovePath(fileToDownload);

            if (zipToUnpack == null)
            {
                var message = Resources.NotOnlineTemplate;
                ShowErrorMessageBox(ref hasUserCancelled, ref zipToUnpack, message);

                if (string.IsNullOrEmpty(zipToUnpack))
                    zipToUnpack = FileManager.UserApplicationDataForThisApplication +
                                  SelectedTemplate.BackingData.ZipName;

                checkOnline = false;
            }

            if (checkOnline)
            {
                if (string.IsNullOrEmpty(fileToDownload))
                {
                    var message = Resources.CantFindProjectOnline;

                    ShowErrorMessageBox(ref hasUserCancelled, ref zipToUnpack, message);
                }
                else
                    shouldTryDownloading = true;
            }

            return zipToUnpack;
        }

        private void ShowErrorMessageBox(ref bool hasUserCancelled, ref string zipToUnpack, string message)
        {
            var mbmb = new MultiButtonMessageBox { MessageText = message };
            mbmb.AddButton(Resources.DefaultFindFile, DialogResult.Yes);
            mbmb.AddButton(Resources.SelectZipUse,  DialogResult.Ok);
            mbmb.AddButton(Resources.NothingDontCreateProject, DialogResult.Cancel);
            var dialogResult = mbmb.ShowMessageBox();

            if (dialogResult == DialogResult.Yes)
            {
                // do nothing, it'll just look in the default location....
            }
            else if (dialogResult == DialogResult.Ok)
            {
                var fileDialog = new OpenFileDialog
                {
                    InitialDirectory = "C:\\",
                    Filter = $"{Resources.ZipFRBTemplate} (*.zip)|*.zip",
                    RestoreDirectory = true
                };

                if (fileDialog.ShowDialog() == true)
                {
                    zipToUnpack = fileDialog.FileName;
                }
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                hasUserCancelled = true;
            }
        }

        private bool GetIfFileNameIsValid(ref string unpackDirectory)
        {
            string whyIsValid = null;

            if (CreateProjectDirectory)
            {
                if (ProjectName.Contains(" "))
                    whyIsValid = Resources.NotSpaceProjectTip;

                unpackDirectory = CombineProjectDirectory;
            }

            if (string.IsNullOrEmpty(whyIsValid) && Directory.Exists(unpackDirectory))
            {
                var hasFiles = Directory.GetFiles(unpackDirectory).Any();

                if (hasFiles)
                    whyIsValid = Resources.Directory + unpackDirectory + Resources.NotEmpty;
            }

            if (!string.IsNullOrEmpty(whyIsValid))
                MessageBox.Show(whyIsValid);

            var isFileNameValid = string.IsNullOrEmpty(whyIsValid);
            return isFileNameValid;

        }

        private string GetFileToDownload()
        {
            if (ProjectType == null)
                throw new NotImplementedException(Resources.MustSelectTemplate);

            return ProjectType.Url;
        }

        public void GetDefaultZipLocationAndStringToReplace(PlatformProjectInfo project, out string zipToUnpack,
            out string stringToReplace)
        {
            GetDefaultZipLocationAndStringToReplace(project, FileManager.UserApplicationDataForThisApplication, out zipToUnpack, out stringToReplace);
        }

        public void GetDefaultZipLocationAndStringToReplace(PlatformProjectInfo project, string templateLocation,
            out string zipToUnpack, out string stringToReplace)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            zipToUnpack = string.Empty;
            stringToReplace = string.Empty;

            if (!Directory.Exists(templateLocation))
                Directory.CreateDirectory(templateLocation);

            zipToUnpack = templateLocation + project.ZipName;
            stringToReplace = project.Namespace;
        }

        private void HandleMakeNewProject()
        {
            var whyIsntValid = GetWhyIsntValid();

            if (!string.IsNullOrEmpty(whyIsntValid))
                MessageBox.Show(whyIsntValid);
            else
            {
                var succeeded = false;
                try
                {
                    succeeded = MakeNewProject();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                if (succeeded)
                    CreateProject.GetInstance().Close();
            }
        }

        public string GetWhyIsntValid()
        {
            string whyIsntValid = null;

            if (UseDifferentNamespace)
            {
                if (string.IsNullOrEmpty(DifferentNamespace))
                    whyIsntValid = Resources.NotEmptyNamespaceTip;
                else if (char.IsDigit(DifferentNamespace[0]))
                    whyIsntValid = Resources.NotNumberNamespaceTip;
                else if (DifferentNamespace.Contains(' '))
                    whyIsntValid = Resources.NotSpaceNamespaceTip;
                else if (DifferentNamespace.IndexOfAny(InvalidNamespaceCharacters) != -1)
                    whyIsntValid = Resources.NotInvalidNamespaceTip +
                                   DifferentNamespace[DifferentNamespace.IndexOfAny(InvalidNamespaceCharacters)];
            }

            if (string.IsNullOrEmpty(whyIsntValid))
                whyIsntValid = ProjectCreationHelper.GetWhyProjectNameIsntValid(ProjectName);

            return whyIsntValid;
        }

        internal void RefreshAvailableTemplates()
        {
            AvailableTemplates.Clear();

            if (SelectedCategory != null)
            {
                if (SelectedCategory.Name == Resources.StarterProjects)
                {
                    foreach (var platformProjectInfo in DataLoader.StarterProjects)
                    {
                        var viewModel = new TemplateViewModel
                        {
                            BackingData = platformProjectInfo
                        };

                        AvailableTemplates.Add(viewModel);
                    }
                }
                else
                {
                    foreach (var platformProjectInfo in DataLoader.EmptyProjects)
                    {
                        var viewModel = new TemplateViewModel
                        {
                            BackingData = platformProjectInfo
                        };

                        AvailableTemplates.Add(viewModel);
                    }
                }
            }

            if (AvailableTemplates.Count != 0)
                SelectedTemplate = AvailableTemplates[0];
        }
    }
}