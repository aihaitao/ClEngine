using System.Collections.Generic;
using ClEngine.Model;
using GalaSoft.MvvmLight;

namespace ClEngine.ViewModel
{
    public class MapEditorViewModel : ViewModelBase
    {
        public List<MapEditorModel> Model;
        public MapEditorViewModel()
        {
            Model = new List<MapEditorModel>
            {
                new MapEditorModel
                {
                    EventId = "主角",
                    Sign = "主角",
                    Property = "主角",
                    Camp = "友方",
                    GridX = 13,
                    GridY = 15
                }
            };
        }
    }
}