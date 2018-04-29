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

        private Queue<string> _messages;

        public Queue<string> Messages
        {
            get => _messages;
            set
            {
                _messages = value;
                RaisePropertyChanged(() => Messages);
            }
        }

        private bool _showGrid;

        public bool ShowGrid
        {
            get => _showGrid;
            set
            {
                _showGrid = value;
                RaisePropertyChanged(() => ShowGrid);
            }
        }
    }
}