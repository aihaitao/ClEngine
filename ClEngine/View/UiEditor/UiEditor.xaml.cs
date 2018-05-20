using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using ClEngine.CoreLibrary.Editor;
using ClEngine.CoreLibrary.Logger;
using ClEngine.Model;
using ClEngine.ViewModel;
using ICSharpCode.WpfDesign.Designer.Services;
using ICSharpCode.WpfDesign.Designer.Xaml;
using Button = System.Windows.Controls.Button;
using CheckBox = System.Windows.Controls.CheckBox;
using Label = System.Windows.Controls.Label;
using ListView = System.Windows.Controls.ListView;
using ProgressBar = System.Windows.Controls.ProgressBar;
using TextBox = System.Windows.Controls.TextBox;

namespace ClEngine
{
    /// <summary>
    /// UiEditor.xaml 的交互逻辑
    /// </summary>
    public partial class UiEditor
    {
	    private static string xaml = @"<Grid 
xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
mc:Ignorable=""d""
x:Name=""rootElement"" Background=""DarkWhite""></Grid>";

        private static string script = @"Main = 
function(event) -- 整数型 1:打开 2:关闭 3:位置被改变
	
	
	
end";
        
        private readonly UiViewModel _uiViewModel;

        public UiEditor()
        {

            InitializeComponent();

            _uiViewModel = new UiViewModel();
	        DataContext = _uiViewModel;

	        WindowListBox.ItemsSource = _uiViewModel.WindowModels;

            WindowListBox.SelectionChanged += WindowListBoxOnSelectionChanged;
        }

        private void WindowListBoxOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (WindowListBox.SelectedItem is WindowModel windowModel)
            {
                LoadCanvas(windowModel.CanvasScript);
                return;
            }

	        DesignSurface.UnloadDesigner();
        }

        private void LoadCanvas(string content)
        {
            try
            {
                var loadSettings = new XamlLoadSettings();
                loadSettings.DesignerAssemblies.Add(GetType().Assembly);

                using (var xmlReader = XmlReader.Create(new StringReader(content)))
                {
                    DesignSurface.LoadDesigner(xmlReader, loadSettings);
                }
			}
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
            
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
	    {
		    ControlsMouseDown(typeof(Button));
	    }

	    private void ControlsMouseDown(Type type)
	    {
		    var tool = new CreateComponentTool(type);
		    DesignSurface.DesignPanel.Context.Services.Tool.CurrentTool = tool;
	    }

	    private void AddWindow_OnClick(object sender, RoutedEventArgs e)
	    {
	        var title = "新游戏窗口";
	        var windowDirName = Path.Combine(EditorRecord.MainViewModel.ProjectPosition, "scripts", "windows");
	        string scriptName;
	        string resultTitle;
	        var num = 1;

            while (true)
            {
                resultTitle = string.Concat(title, num);
                scriptName = GetScriptName(resultTitle);
                if (!File.Exists(scriptName))
                    break;
                num++;
            }

            var windowModel = new WindowModel
		    {
			    DragDrop = true,
			    Title = resultTitle,
                Width = 420,
                Height = 340,
                X = 0,
                Y = 0,
                CanvasScript = xaml,
            };
	        if (!Directory.Exists(windowDirName))
	            Directory.CreateDirectory(windowDirName);

	        using (var fileStream = File.Create(scriptName))
	        {
	            using (var streamWriter = new StreamWriter(fileStream))
	            {
	                streamWriter.Write(script);
	            }
            }
		    _uiViewModel.WindowModels.Add(windowModel);
		}

        private string GetScriptName(string title)
        {
            var windowDirName = Path.Combine(EditorRecord.MainViewModel.ProjectPosition, "scripts", "windows");
            return Path.Combine(windowDirName, string.Concat(title, ".lua"));
        }

        private void LabelBase_OnClick(object sender, RoutedEventArgs e)
        {
            ControlsMouseDown(typeof(Label));
        }

        private void ImageBase_OnClick(object sender, RoutedEventArgs e)
        {
            ControlsMouseDown(typeof(Image));
        }

        private void InputBase_OnClick(object sender, RoutedEventArgs e)
        {
            ControlsMouseDown(typeof(TextBox));
        }

        private void ListBase_OnClick(object sender, RoutedEventArgs e)
        {
            ControlsMouseDown(typeof(ListView));
        }

        private void ProgressBase_OnClick(object sender, RoutedEventArgs e)
        {
            ControlsMouseDown(typeof(ProgressBar));
        }

        private void CheckBoxBase_OnClick(object sender, RoutedEventArgs e)
        {
            ControlsMouseDown(typeof(CheckBox));
        }

        private void SliderBase_OnClick(object sender, RoutedEventArgs e)
        {
            ControlsMouseDown(typeof(Slider));
        }

        private void ScriptEditor_OnClick(object sender, RoutedEventArgs e)
        {
            var item = WindowListBox.SelectedItem;
            var windowDirName = Path.Combine(EditorRecord.MainViewModel.ProjectPosition, "scripts", "windows");
            if (item != null)
            {
                var scriptName = Path.Combine(windowDirName, string.Concat((item as WindowModel)?.Title, ".lua"));
                var floatScriptEditor = new FloatScriptEditor(scriptName);
                floatScriptEditor.ShowDialog();
            }
        }

        private void DeleteWindow_OnClick(object sender, RoutedEventArgs e)
        {
            var item = WindowListBox.SelectedItem;
	        var windowDirName = Path.Combine(EditorRecord.MainViewModel.ProjectPosition, "scripts", "windows");
			if (item != null)
			{
				var model = item as WindowModel;
				var scriptName = Path.Combine(windowDirName, string.Concat(model?.Title, ".lua"));

				if (File.Exists(scriptName))
					File.Delete(scriptName);

                _uiViewModel.WindowModels.Remove(model);
            }
        }
    }
}
