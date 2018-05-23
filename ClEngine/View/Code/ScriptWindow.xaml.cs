using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using ClEngine.CoreLibrary.Editor;
using GalaSoft.MvvmLight.Messaging;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Search;

// ReSharper disable once CheckNamespace
namespace ClEngine
{
    /// <summary>
    /// ScriptWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ScriptWindow
    {
        public TextEditor ScriptEditor => TextEditor;

        public ScriptWindow()
        {
            IHighlightingDefinition customHighlightingDefinition;

            using (var s = typeof(MainWindow).Assembly.GetManifestResourceStream("ClEngine.Lua.xshd"))
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

            InitializeComponent();

            SearchPanel.Install(TextEditor);

            TextEditor.TextArea.TextEntering += TextAreaOnTextEntering;

            Messenger.Default.Register<string>(this, "SaveScript", SaveScript, true);
        }
        
        private void CodeViewer_OnClick(object sender, RoutedEventArgs e)
        {
            if (CodeListBox.SelectedItem != null && CodeListBox.SelectedItem is TextBlock textBlock)
            {
                var scriptPath = Path.Combine(EditorRecord.MainViewModel.ProjectPosition, "scripts");
                if (Equals(textBlock.Text, Properties.Resources.Startup))
                {
                    LoadDocument(Path.Combine(scriptPath, "init.lua"));
                }
                else if (Equals(textBlock.Text, Properties.Resources.GameStart))
                {
                    LoadDocument(Path.Combine(scriptPath, "start.lua"));
                }
                else if (Equals(textBlock.Text, Properties.Resources.MapSwitch))
                {
                    LoadDocument(Path.Combine(scriptPath, "map.lua"));
                }
                else if (Equals(textBlock.Text, Properties.Resources.GameTask))
                {
                    LoadDocument(Path.Combine(scriptPath, "task.lua"));
                }
                else if (Equals(textBlock.Text, Properties.Resources.KeyEvent))
                {
                    LoadDocument(Path.Combine(scriptPath, "input.lua"));
                }
                else if (Equals(textBlock.Text, Properties.Resources.ItemDrop))
                {
                    LoadDocument(Path.Combine(scriptPath, "itemdrop.lua"));
                }
                else if (Equals(textBlock.Text, Properties.Resources.SkillEvent))
                {
                    LoadDocument(Path.Combine(scriptPath, "skillgrid.lua"));
                }
                else if (Equals(textBlock.Text, Properties.Resources.WindowClose))
                {
                    LoadDocument(Path.Combine(scriptPath, "windowclose.lua"));
                }
                else if (Equals(textBlock.Text, Properties.Resources.OtherEvent))
                {
                    LoadDocument(Path.Combine(scriptPath, "otherevent.lua"));
                }
            }
        }

        public CompletionWindow CompletionWindow;
        private void TextAreaOnTextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && CompletionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    CompletionWindow.CompletionList.RequestInsertion(e);
                }
            }
        }

        private string FileName { get; set; }

        private void LoadDocument(string filenamme)
        {
            if (TextEditor.IsModified)
            {
                var result = System.Windows.Forms.MessageBox.Show(@"监测到文本发生更改,是否保存？", @"脚本未保存",
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
            }
        }


        // ReSharper disable once UnusedMember.Local
        private void SwitchOldXshdFile(string oldFile, string newFile)
        {
            XshdSyntaxDefinition xshd;
            using (var reader = new XmlTextReader(oldFile))
            {
                xshd = HighlightingLoader.LoadXshd(reader);
            }

            using (var writer = new XmlTextWriter(newFile, System.Text.Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                new SaveXshdVisitor(writer).WriteDefinition(xshd);
            }
        }
    }
}
