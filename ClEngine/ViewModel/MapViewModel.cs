using System;
using System.Windows.Input;
using ClEngine.Model;
using ClEngine.Tiled.MapEnum;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace ClEngine.ViewModel
{
    public class MapViewModel : ViewModelBase
    {
        public MapModel Model { get; set; }
        public ICommand CreateMapCommand { get; set; }
        public bool IsSelected { get; set; }
        public string FixedX { get; set; }
        public string FixedY { get; set; }
        public string BlockWidth { get; set; }
        public string BlockHeight { get; set; } 
        public MapOrientation MapDirection { get; set; }
        public RenderOrder RenderOrder { get; set; }

        public MapViewModel()
        {
            Model = new MapModel();
            CreateMapCommand = new RelayCommand<bool>(CreateMapExecute);
        }

        private void CreateMapExecute(bool isSelected)
        {
            if (isSelected)
            {
                Tiled.Map map = new Tiled.Map();
                int.TryParse(FixedX, out var width);
                int.TryParse(FixedY, out var height);
                int.TryParse(BlockWidth, out var blockWidth);
                int.TryParse(BlockHeight, out var blockHeight);
                Enum.TryParse(MapDirection.ToString(), out MapOrientation orientation);
                Enum.TryParse(RenderOrder.ToString(), out RenderOrder renderOrder);
                map.Width = width;
                map.Height = height;
                map.Tilewidth = blockWidth;
                map.Tileheight = blockHeight;
                map.Orientation = orientation;
                map.Renderorder = renderOrder;

                // TODO: Create Map
            }
        }
    }
}