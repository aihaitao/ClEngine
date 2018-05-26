using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ClEngine.CoreLibrary.Editor;
using GalaSoft.MvvmLight;

namespace ClEngine.Model
{
    public enum NameSpaceType
    {
        [Description("system")]
        System,
        [Description("content")]
        Content,
    }

    public enum DocumentType
    {
        [EditorDescription("Attribute")]
        Attribute,
        [EditorDescription("Method")]
        Method,
    }

    public class DocumentModel : ObservableObject
    {
        public ObservableCollection<DocumentChildModel> ChildModels { get; set; }
        public string Name { get;set; }

        public DocumentModel()
        {
            ChildModels = new ObservableCollection<DocumentChildModel>();
        }
    }

    public class DocumentChildModel
    {
        public string Name { get; set; }
        public NameSpaceType NameSpace { get; set; }
        public DocumentType Type { get; set; }
        public string Example { get; set; }
    }
}