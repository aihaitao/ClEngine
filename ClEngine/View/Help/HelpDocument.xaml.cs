using System.Windows.Controls;
using ClEngine.ViewModel;

namespace ClEngine
{
    /// <summary>
    /// HelpDocument.xaml 的交互逻辑
    /// </summary>
    public partial class HelpDocument : UserControl
    {
        public HelpDocument()
        {
            InitializeComponent();

            var viewModel = (DocumentViewModel) DataContext;
            TreeView.ItemsSource = viewModel.GetTrees(0, viewModel.GetDocuments());
        }
    }
}
