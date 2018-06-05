using System.Windows.Controls;

namespace ClEngine.CoreLibrary.Manager
{
    public interface IFindManager
    {
        TreeViewItem TreeNodeForDirectoryOrEntityNode(string containingDirection, TreeViewItem containingNode);

        string GlobalContentFilesPath { get; }
    }
}