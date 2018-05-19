using System.ComponentModel;
using System.Windows;

namespace ClEngine.View.Messages
{
    /// <summary>
    /// MessageProperty.xaml 的交互逻辑
    /// </summary>
    public partial class MessageProperty : Window
    {
        public bool MessagePropertyIsShow;
        public MessageProperty()
        {
            MessagePropertyIsShow = false;
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
            MessagePropertyIsShow = false;
        }
    }
}
