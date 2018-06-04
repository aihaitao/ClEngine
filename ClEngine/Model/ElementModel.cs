using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace ClEngine.Model
{
    public class ElementModel : ObservableObject
    {
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        private ObservableCollection<ElementModel> _children;
        public ObservableCollection<ElementModel> Children
        {
            get => _children;
            set
            {
                _children = value;
                RaisePropertyChanged(() => Children);
            }
        }

        private ElementModel _parent;
        public ElementModel Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                RaisePropertyChanged(() => Parent);
            }
        }

        private string _icon;
        public string Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                RaisePropertyChanged(() => Icon);
            }
        }

        public ElementModel()
        {
            Children = new ObservableCollection<ElementModel>();

        }
    }
}