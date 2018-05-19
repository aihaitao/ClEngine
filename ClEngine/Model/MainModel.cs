using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace ClEngine.Model
{
    public class MainModel : ObservableObject
    {
        private bool _isLoadProject;
        public bool IsLoadProject
        {
            set
            {
                _isLoadProject = value;
                RaisePropertyChanged(() => IsLoadProject);
            }
            get => _isLoadProject;
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

        private ProjectInfo _projectInfo;

        public ProjectInfo ProjectInfo
        {
            get => _projectInfo;
            set
            {
                _projectInfo = value;
                RaisePropertyChanged(() => ProjectInfo);
            }
        }
    }
}