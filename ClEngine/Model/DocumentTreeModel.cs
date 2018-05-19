using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace ClEngine.Model
{
    public class DocumentTreeModel : ObservableObject
    {
        public List<DocumentTreeModel> Nodes { get; set; }

        public DocumentTreeModel()
        {
            Nodes = new List<DocumentTreeModel>();
            ParentId = 0;
        }

        public int id { get; set; }
        public string deptName { get; set; }
        public int ParentId { get; set; }
    }
}