using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using ClEngine.Model;
using ClEngine.Properties;
using GalaSoft.MvvmLight;

namespace ClEngine.ViewModel
{
    public class ElementViewModel : ViewModelBase
    {
        internal static List<string> DirectoriesToIgnore = new List<string>();
        internal static TreeViewItem mEntityNode;
        internal static TreeViewItem mScreenNode;
        internal static TreeViewItem mGlobalContentNode;

        public static TreeViewItem GlobalContentFileNode => mGlobalContentNode;

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

            mEntityNode = new TreeViewItem
            {
                Header = Resources.Entities,
            };
            mScreenNode = new TreeViewItem
            {
                Header = Resources.Screens,
            };
            mGlobalContentNode = new TreeViewItem
            {
                Header = Resources.GlobalContentFile
            };

            Initialize(mEntityNode, mScreenNode, mGlobalContentNode);
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