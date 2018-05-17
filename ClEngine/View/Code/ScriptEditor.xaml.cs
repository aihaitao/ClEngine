using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using ClEngine.CoreLibrary;
using ClEngine.CoreLibrary.Logger;
using GalaSoft.MvvmLight.Messaging;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Search;
using UserControl = System.Windows.Controls.UserControl;

namespace ClEngine
{
    /// <summary>
    /// ScriptEditor.xaml 的交互逻辑
    /// </summary>
    public partial class ScriptEditor : UserControl
    {
        public ScriptEditor()
        {
            IHighlightingDefinition customHighlightingDefinition;

            using (Stream s = typeof(MainWindow).Assembly.GetManifestResourceStream("ClEngine.CustomHighlighting.xshd"))
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

            //SwitchOldXshdFile("XML.xshd", "XML-New.xshd");

            InitializeComponent();

            SearchPanel.Install(TextEditor);

			TextEditor.TextArea.TextEntering += TextAreaOnTextEntering;

            Messenger.Default.Register<string>(this, "SaveScript", SaveScript);
            Messenger.Default.Register<string>(this, "LoadDocument", LoadDocument);
            Messenger.Default.Register<string>(this, "LoadFloatDocument", LoadFloatDocument);
        }

	    private CompletionWindow _completionWindow;
	    private void TextAreaOnTextEntering(object sender, TextCompositionEventArgs e)
	    {
		    if (e.Text.Length > 0 && _completionWindow != null)
		    {
			    if (!char.IsLetterOrDigit(e.Text[0]))
			    {
					_completionWindow.CompletionList.RequestInsertion(e);
			    }
		    }
	    }

	    private string FileName { get; set; }

        private void LoadDocument(string filenamme)
        {
            if (TextEditor.IsModified)
            {
                var result = MessageBox.Show(@"监测到文本发生更改,是否保存？", @"脚本未保存",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    TextEditor.Save(FileName);
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            TextEditor.Load(filenamme);
            FileName = filenamme;
        }

        private void LoadFloatDocument(string filenamme)
        {
            if (TextEditor.IsModified)
            {
                var result = MessageBox.Show(@"监测到文本发生更改,是否保存？", @"脚本未保存",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    TextEditor.Save(FileName);
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            TextEditor.Load(filenamme);
            FileName = filenamme;
        }

        public void SaveScript(string emptyArg)
        {
            if (TextEditor.IsModified && File.Exists(FileName))
            {
                TextEditor.Save(FileName);
                Logger.Log("脚本已保存");
            }
            else
                Logger.Warn("未发现有更改，忽略该请求");
        }


        private void SwitchOldXshdFile(string oldFile, string newFile)
        {
            XshdSyntaxDefinition xshd;
            using (XmlTextReader reader = new XmlTextReader(oldFile))
            {
                xshd = HighlightingLoader.LoadXshd(reader);
            }

            using (XmlTextWriter writer = new XmlTextWriter(newFile, System.Text.Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                new SaveXshdVisitor(writer).WriteDefinition(xshd);
            }
        }
    }
}
