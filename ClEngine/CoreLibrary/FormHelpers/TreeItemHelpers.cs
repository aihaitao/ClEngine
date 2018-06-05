using System.Windows.Controls;
using ClEngine.CoreLibrary.Controls;
using ClEngine.CoreLibrary.Plugins.ExportedImplementations;
using ClEngine.ViewModel;
using FlatRedBall.Glue.SaveClasses;

namespace ClEngine.CoreLibrary.FormHelpers
{
    public static class TreeItemHelpers
    {
        public static TreeViewItem Root(this TreeViewItem treeNodeInQuestion)
        {
            while (true)
            {
                if (treeNodeInQuestion == null)
                {
                    return null;
                }

                if (treeNodeInQuestion.Parent == null) return treeNodeInQuestion;
                treeNodeInQuestion = treeNodeInQuestion.Parent as TreeViewItem;
            }
        }

        public static bool IsGlobalContentContainerNode(this TreeViewItem treeNodeInQuestion)
        {
            return Equals(treeNodeInQuestion, ElementViewModel.GlobalContentFileNode);
        }

        public static bool IsDirectoryNode(this TreeViewItem treeNodeInQuestion)
        {
            if (treeNodeInQuestion.Parent == null)
                return false;

            if (treeNodeInQuestion is EntityTreeNode)
                return false;

            if (treeNodeInQuestion.Tag != null)
            {
                return false;
            }

            if (treeNodeInQuestion.IsReferencedFile())
            {
                return false;
            }

            if ((treeNodeInQuestion.Parent as TreeViewItem).IsRootEntityNode() ||
                (treeNodeInQuestion.Parent as TreeViewItem).IsGlobalContentContainerNode())
                return true;

            return !treeNodeInQuestion.IsEntityNode() && !treeNodeInQuestion.IsScreenNode() &&
                   (treeNodeInQuestion.Parent as TreeViewItem).IsFilesContainerNode() ||
                   (treeNodeInQuestion.Parent as TreeViewItem).IsDirectoryNode();
        }

        public static bool IsFilesContainerNode(this TreeViewItem treeNodeInQuestion)
        {
            var parentTreeNode = treeNodeInQuestion.Parent;
            return (string) treeNodeInQuestion.Header == "Files" && parentTreeNode != null &&
                   ((parentTreeNode as TreeViewItem).IsEntityNode() || (parentTreeNode as TreeViewItem).IsScreenNode());
        }

        public static bool IsScreenNode(this TreeViewItem treeNodeInQuestion)
        {
            return treeNodeInQuestion is ScreenTreeNode;
        }

        public static bool IsEntityNode(this TreeViewItem treeNodeInQuestion)
        {
            return treeNodeInQuestion is EntityTreeNode;
        }

        public static bool IsReferencedFile(this TreeViewItem treeNodeInQuestion)
        {
            return treeNodeInQuestion?.Tag is ReferencedFileSave;
        }

        public static bool IsRootEntityNode(this TreeViewItem treeNodeInQuestion)
        {
            return (string) treeNodeInQuestion.Header == "Entities" && treeNodeInQuestion.Parent == null;
        }

        public static bool IsRootScreenNode(this TreeViewItem treeNodeInQuestion)
        {
            return (string) treeNodeInQuestion.Header == "Screens" && treeNodeInQuestion.Parent == null;
        }

        public static bool IsFolderInFilesContainerNode(this TreeViewItem treeNodeInQuestion)
        {
            return treeNodeInQuestion.Tag == null && treeNodeInQuestion.Parent is TreeViewItem parentTreeNode &&
                   (parentTreeNode.IsFilesContainerNode() || parentTreeNode.IsFolderInFilesContainerNode());

        }

        public static bool IsElementNode(this TreeViewItem treeNodeInQuestion)
        {
            return treeNodeInQuestion.IsScreenNode() || treeNodeInQuestion.IsEntityNode();
        }

        public static string GetRelativePath(this TreeViewItem treeNodeInQuestion)
        {
            #region Directory tree node
            if (treeNodeInQuestion.IsDirectoryNode())
            {
                if ((treeNodeInQuestion.Parent as TreeViewItem).IsRootEntityNode())
                {
                    return "Entities/" + treeNodeInQuestion.Header + "/";

                }
                if ((treeNodeInQuestion.Parent as TreeViewItem).IsRootScreenNode())
                {
                    return "Screens/" + treeNodeInQuestion.Header + "/";

                }

                if (!(treeNodeInQuestion.Parent as TreeViewItem).IsGlobalContentContainerNode())
                    return (treeNodeInQuestion.Parent as TreeViewItem).GetRelativePath() + treeNodeInQuestion.Header +
                           "/";
                var contentDirectory = ProjectManager.MakeAbsolute("GlobalContent", true);

                var returnValue = contentDirectory + treeNodeInQuestion.Header;
                if (treeNodeInQuestion.IsDirectoryNode())
                {
                    returnValue += "/";
                }
                // But we want to make this relative to the project, so let's do that
                returnValue = ProjectManager.MakeRelativeContent(returnValue);

                return returnValue;
// It's a tree node, so make it have a "/" at the end
            }
            #endregion

            #region Global content container

            if (treeNodeInQuestion.IsGlobalContentContainerNode())
            {
                var returnValue = GlueState.Self.Find.GlobalContentFilesPath;


                // But we want to make this relative to the project, so let's do that
                returnValue = ProjectManager.MakeRelativeContent(returnValue);



                return returnValue;
            }
            #endregion
            if (treeNodeInQuestion.IsFilesContainerNode())
            {
                var valueToReturn = (treeNodeInQuestion.Parent as TreeViewItem).GetRelativePath();


                return valueToReturn;
            }
            if (treeNodeInQuestion.IsFolderInFilesContainerNode())
            {
                return (treeNodeInQuestion.Parent as TreeViewItem).GetRelativePath() + treeNodeInQuestion.Header + "/";
            }
            if (treeNodeInQuestion.IsElementNode())
            {
                return ((IElement)treeNodeInQuestion.Tag).Name + "/";
            }
            if (treeNodeInQuestion.IsReferencedFile())
            {
                var toReturn = (treeNodeInQuestion.Parent as TreeViewItem).GetRelativePath() + treeNodeInQuestion.Header;
                toReturn = toReturn.Replace("/", "\\");
                return toReturn;
            }
// Improve this to handle embeded stuff
            var textToReturn = (string)treeNodeInQuestion.Header;

            if (string.IsNullOrEmpty(FlatRedBall.IO.FileManager.GetExtension(textToReturn)))
            {
                textToReturn += "/";
            }

            return textToReturn;
        }
    }
}