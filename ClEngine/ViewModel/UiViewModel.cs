using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using ClEngine.CoreLibrary.Editor;
using ClEngine.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json.Linq;

namespace ClEngine.ViewModel
{
	public class UiViewModel : ViewModelBase
	{
        public ObservableCollection<WindowModel> WindowModels;
	    private string WindowConfig { get; set; }


	    public UiViewModel()
		{
		    WindowModels = new ObservableCollection<WindowModel>();

		    WindowModels.CollectionChanged += WindowModelsOnCollectionChanged;

            Messenger.Default.Register<string>(this, "LoadUiConfig", LoadConfig, true);
        }

	    private void WindowModelsOnCollectionChanged(object sender,
	        NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
	    {
	        WindowModels.SaveJson(WindowConfig);
	    }

	    private void LoadConfig(string e)
	    {
	        WindowConfig = Path.Combine(EditorRecord.MainViewModel.ProjectPosition, "windows.conf");

	        WindowModels.CollectionChanged -= WindowModelsOnCollectionChanged;

	        if (File.Exists(WindowConfig))
	        {
	            var models = JsonHelper.LoadJson(WindowConfig) as JArray;
	            ListToObservableCollection(models?.ToList());
	        }

            WindowModels.CollectionChanged += WindowModelsOnCollectionChanged;
        }

	    private void ListToObservableCollection(IEnumerable<JToken> models)
	    {
	        foreach (var model in models)
	        {
                var windowModel = new WindowModel()
                {
                    CanvasScript = model["CanvasScript"].Value<string>(),
                    DragDrop = model["DragDrop"].Value<bool>(),
                    Title = model["Title"].Value<string>(),
                    TopVisible = model["TopVisible"].Value<bool>(),
                    Image = model["Image"].Value<Image>(),
                    MouseThrough = model["MouseThrough"].Value<bool>(),
                    NinePattern = model["NinePattern"].Value<bool>(),
                    X = model["X"].Value<int>(),
                    Y = model["Y"].Value<int>(),
                    Width = model["Width"].Value<int>(),
                    Height = model["Height"].Value<int>(),
                    Visible = model["Visible"].Value<bool>(),
                };
                WindowModels.Add(windowModel);
	        }
	    }
	}
}