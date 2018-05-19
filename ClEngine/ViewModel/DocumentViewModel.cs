using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;
using ClEngine.Model;
using ClEngine.Properties;

namespace ClEngine.ViewModel
{
    public class DocumentViewModel : ViewModelBase
    {
        public ICommand HelpCommand { get; set; }
        public List<DocumentModel> DocumentModels { get; set; }

        public DocumentViewModel()
        {
            HelpCommand = new RelayCommand<string>(Execute);
            DocumentModels = new List<DocumentModel>();
            GetDocument();
        }

        private void Execute(string obj)
        {
            foreach (var documentModel in DocumentModels)
            {
                if (string.Equals(documentModel.Name, obj))
                {

                }
            }
        }

        public void GetDocument()
        {
            DocumentModels.Clear();

            var systemWidth = new DocumentModel
            {
                Name = "system.width",
                Example = "",
                NameSpace = NameSpaceType.System,
                Type = DocumentType.Attribute,
            };
            DocumentModels.Add(systemWidth);

            var systemHeight = new DocumentModel
            {
                Name = "system.height",
                Example = "",
                NameSpace = NameSpaceType.System,
                Type = DocumentType.Attribute,
            };
            DocumentModels.Add(systemHeight);
        }

        public List<DocumentTreeModel> GetDocuments()
        {
            var dclst = new List<DocumentTreeModel>
            {
                new DocumentTreeModel{id = 1, deptName = "ClEngine", ParentId = 0},
                new DocumentTreeModel{id = 2, deptName = Resources.Real_timeCombatItems, ParentId = 1},
                new DocumentTreeModel{id = 3, deptName = Resources.InterfaceDocumentation, ParentId = 2},
                new DocumentTreeModel{id = 4, deptName = "system", ParentId = 3},
            };
            
            return dclst;
        }

        public List<DocumentTreeModel> GetTrees(int parentid, List<DocumentTreeModel> nodes)
        {
            var mainNodes = nodes.Where(x => x.ParentId == parentid).ToList();
            var otherNodes = nodes.Where(x => x.ParentId != parentid).ToList();
            foreach (var documentTreeModel in mainNodes)
            {
                documentTreeModel.Nodes = GetTrees(documentTreeModel.id, otherNodes);
            }

            return mainNodes;
        }
    }
}