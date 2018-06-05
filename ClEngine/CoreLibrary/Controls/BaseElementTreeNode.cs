using System.Windows.Controls;
using FlatRedBall.Glue.SaveClasses;

namespace ClEngine.CoreLibrary.Controls
{
    public class BaseElementTreeNode : TreeViewItem
    {
        public const bool UseIcons = true;
    }

    public abstract class BaseElementTreeNode<T> : BaseElementTreeNode where T : IElement
    {

    }
}