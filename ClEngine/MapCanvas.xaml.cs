using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ClEngine
{
    /// <summary>
    /// MapCanvas.xaml 的交互逻辑
    /// </summary>
    public partial class MapCanvas
    {
        private const int gridWidth = 30;
        private const int gridHeight = 15;
        public MapCanvas()
        {
            InitializeComponent();
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            GridLineCarrier.Children.Clear();
            var widthNum = Math.Ceiling(MapImage.ActualWidth / gridWidth);
            for (int i = 0; i < widthNum; i++)
            {
                var drawWidthLine = new Line
                {
                    Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                    StrokeThickness = 1,
                    Height = MapImage.ActualHeight,
                    Width = MapImage.ActualWidth,
                    X1 = gridWidth * i,
                    Y1 = 0,
                    X2 = gridWidth * i,
                    Y2 = MapImage.ActualHeight,
                };
                GridLineCarrier.Children.Add(drawWidthLine);

                var heightNum = Math.Ceiling(MapImage.ActualHeight / gridHeight);
                for (int j = 0; j < heightNum; j++)
                {
                    var drawHeightLine = new Line
                    {
                        Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                        StrokeThickness = 1,
                        Height = MapImage.ActualHeight,
                        Width = MapImage.ActualWidth,
                        X1 = 0,
                        Y1 = gridHeight * j,
                        X2 = MapImage.ActualWidth,
                        Y2 = gridHeight * j,
                    };
                    GridLineCarrier.Children.Add(drawHeightLine);
                }
            }
        }
    }
}
