using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using ClEngine.Model;
using GalaSoft.MvvmLight;

namespace ClEngine.ViewModel
{
    public class ElementViewModel : ViewModelBase
    {
        internal static List<string> DirectoriesToIgnore = new List<string>();

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

        public ElementViewModel()
        {
            Children = new ObservableCollection<ElementModel>();

            var entityItem = new TreeViewItem
            {
                Header = Properties.Resources.Entities,
            };
            var sceneItem = new TreeViewItem
            {
                Header = Properties.Resources.Screens,
            };
            var globalItem = new TreeViewItem
            {
                Header = Properties.Resources.GlobalContentFile
            };

            Initialize(entityItem, sceneItem, globalItem);
        }


        public void Initialize(TreeViewItem entityNode, TreeViewItem screenNode,
            TreeViewItem gloablContentNode)
        {
            DirectoriesToIgnore.Add(".svn");

            var entityModel = new ElementModel
            {
                Icon = "/Icons/master_entity.png",
                Name = entityNode.Header.ToString(),
            };
            Children.Add(entityModel);

            var screenModel = new ElementModel
            {
                Icon = "/Icons/master_screen.png",
                Name = screenNode.Header.ToString(),
            };
            Children.Add(screenModel);

            var globalCotentModel = new ElementModel
            {
                Icon = "/Icons/master_file.png",
                Name = gloablContentNode.Header.ToString(),
            };
            Children.Add(globalCotentModel);
        }
    }
    
}