using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using ClEngine.CoreLibrary.Logger;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Search;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ClEngine
{
    /// <summary>
    /// FloatScriptEditor.xaml 的交互逻辑
    /// </summary>
    public partial class FloatScriptEditor
    {
        public FloatScriptEditor(string fileName)
        {
            IHighlightingDefinition customHighlightingDefinition;

            using (Stream s = typeof(MainWindow).Assembly.GetManifestResourceStream("ClEngine.Lua.xshd"))
            {
                if (s == null)
                    throw new InvalidOperationException("没有找到对应资源");
                using (var reader = new XmlTextReader(s))
                {
                    customHighlightingDefinition =
                        HighlightingLoader.Load(reader,
                            HighlightingManager.Instance);
                }
            }

            HighlightingManager.Instance.RegisterHighlighting("Lua", new[] { ".lua" }, customHighlightingDefinition);

            //SwitchOldXshdFile("CustomHighlighting.xshd", "CustomHighlighting.xshd");

            InitializeComponent();

            SearchPanel.Install(TextEditor);
            
            LoadDocument(fileName);
        }

        private string FileName { get; set; }

        private void LoadDocument(string filenamme)
        {
            if (TextEditor.IsModified)
            {
                var result = MessageBox.Show(@"监测到文本发生更改,是否保存？", @"脚本未保存",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    TextEditor.Save(FileName);
                }
                else if (result == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
            }

            TextEditor.Load(filenamme);
            FileName = filenamme;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            if (TextEditor.IsModified && File.Exists(FileName))
            {
                TextEditor.Save(FileName);
                Logger.Log("脚本已保存!");
            }
            else
                Logger.Warn("未发现有更改，忽略该请求");
            
            Close();
        }
    }
}
