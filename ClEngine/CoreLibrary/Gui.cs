using System;
using System.Windows;
using ClEngine.Properties;
using ClEngine.View.Project;
using NewProjectCreator.Views;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

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

        public static bool TryShowDialog(MultiButtonMessageBox form, out DialogResult result)
        {
            result = DialogResult.OK;
            if (ShowGui)
            {
                // Can't be invoked async.
                //mMenuStrip.Invoke((MethodInvoker)delegate
                //{
                result = form.ShowMessageBox();
                //});
                return true;
            }

            return false;

        }
    }
}