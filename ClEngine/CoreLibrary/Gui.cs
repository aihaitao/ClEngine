using System;
using ClEngine.Properties;
using Xceed.Wpf.Toolkit;

namespace ClEngine.CoreLibrary
{
    public static class Gui
    {
        public static bool ShowGui { get; set; }
        

        static Gui()
        {
            ShowGui = true;
        }

        public static void ShowException(string text, string caption, Exception ex)
        {
            if (ShowGui)
            {
                MessageBox.Show($"{text}\n\n\n{Resources.Details}:\n\n{ex}", caption);
            }
            else
            {
                throw new Exception(text, ex);
            }
        }
    }
}