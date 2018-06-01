using System.Collections.Generic;
using System.Windows.Controls;

namespace ClEngine.View.Project
{
    public delegate void RefAction<T>(ref T value);

    public enum DialogResult
    {
        Yes,
        Ok,
        Cancel,
    }

    /// <summary>
    /// MultiButtonMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class MultiButtonMessageBox
    {
        internal List<Button> Buttons = new List<Button>();

        public object ClickedTag { get; private set; }

        public DialogResult Result { get; set; }

        public string MessageText
        {
            set => TextBlock.Text = value;
        }
        public MultiButtonMessageBox()
        {
            InitializeComponent();
        }
        
        public void AddButton(string text, DialogResult result)
        {
            var button = new Button
            {
                Content = text
            };
            button.Click += (sender, args) =>
            {
                ClickedTag = ((Button)sender).Tag;
                Result = result;
                Close();
            };

            Buttons.Add(button);

            StackPanel.Children.Add(button);
        }

        public DialogResult ShowMessageBox()
        {
            ShowDialog();

            return Result;
        }
    }
}
