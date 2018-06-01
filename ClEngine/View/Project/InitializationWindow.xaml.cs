using System.Windows;

namespace ClEngine.View.Project
{
    /// <summary>
    /// InitializationWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InitializationWindow : Window
    {
        public string Message
        {
            set
            {
                TopLevelLabel.Text = value;
                SubLabel.Text = "";
            }
        }

        public string SubMessage
        {
            set => SubLabel.Text = value;
        }

        public InitializationWindow()
        {
            InitializeComponent();
        }
    }
}
