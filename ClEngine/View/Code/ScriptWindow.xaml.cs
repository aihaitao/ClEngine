using System.IO;
using System.Windows;
using System.Windows.Controls;
using ClEngine.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace ClEngine
{
    /// <summary>
    /// ScriptWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ScriptWindow
    {
        private ScriptViewModel _scriptModel;
        public ScriptWindow()
        {
            InitializeComponent();

            _scriptModel = new ScriptViewModel();
            DataContext = _scriptModel;
        }
        
        private void CodeViewer_OnClick(object sender, RoutedEventArgs e)
        {
            if (CodeListBox.SelectedItem != null && CodeListBox.SelectedItem is TextBlock textBlock)
            {
                var scriptPath = Path.Combine(MainWindow.ProjectPosition, "scripts");
                switch (textBlock.Text)
                {
                    case "启动":
                        Messenger.Default.Send(Path.Combine(scriptPath, "init.lua"),"LoadDocument");
                        break;
                    case "游戏开始":
                        Messenger.Default.Send(Path.Combine(scriptPath, "start.lua"), "LoadDocument");
                        break;
                    case "地图切换":
                        Messenger.Default.Send(Path.Combine(scriptPath, "map.lua"), "LoadDocument");
                        break;
                    case "游戏任务":
                        Messenger.Default.Send(Path.Combine(scriptPath, "task.lua"), "LoadDocument");
                        break;
                    case "按键事件":
                        Messenger.Default.Send(Path.Combine(scriptPath, "input.lua"), "LoadDocument");
                        break;
                    case "物品掉落":
                        Messenger.Default.Send(Path.Combine(scriptPath, "itemdrop.lua"), "LoadDocument");
                        break;
                    case "技能格子":
                        Messenger.Default.Send(Path.Combine(scriptPath, "skillgrid.lua"), "LoadDocument");
                        break;
                    case "窗口关闭":
                        Messenger.Default.Send(Path.Combine(scriptPath, "windowclose.lua"), "LoadDocument");
                        break;
                    case "其他事件":
                        Messenger.Default.Send(Path.Combine(scriptPath, "otherevent.lua"), "LoadDocument");
                        break;
                }
            }
        }
    }
}
