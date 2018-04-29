using System;
using System.Windows.Controls;
using GalaSoft.MvvmLight;

namespace ClEngine.Model
{
    [Serializable]
	public class WindowModel : ObservableObject
	{
		private string _title;
		public string Title
		{
			get => _title;
			set
			{
				_title = value;
				RaisePropertyChanged(() => Title);
			}
		}

		private Image _image;

		public Image Image
		{
			get => _image;
			set
			{
				_image = value;
				RaisePropertyChanged(() => _image);
			}
		}

	    private int _width;

	    public int Width
	    {
	        get => _width;
	        set
	        {
	            _width = value;
	            RaisePropertyChanged(() => Width);
	        }
	    }

	    private int _height;

	    public int Height
	    {
	        get => _height;
	        set
	        {
	            _height = value;
	            RaisePropertyChanged(() => Height);
	        }
	    }

        private int _x;
		public int X
		{
			get => _x;
			set
			{
			    _x = value;
				RaisePropertyChanged(() => X);
			}
		}

	    private int _y;
	    public int Y
	    {
	        get => _y;
	        set
	        {
	            _y = value;
	            RaisePropertyChanged(() => Y);
	        }
	    }

        private bool _visible;

		public bool Visible
		{
			get => _visible;
			set
			{
				_visible = value;
				RaisePropertyChanged(() => Visible);
			}
		}

		private bool _dragDrop;

		public bool DragDrop
		{
			get => _dragDrop;
			set
			{
				_dragDrop = value;
				RaisePropertyChanged(() => DragDrop);
			}
		}

		private bool _topVisible;

		public bool TopVisible
		{
			get => _topVisible;
			set
			{
				_topVisible = value;
				RaisePropertyChanged(() => TopVisible);
			}
		}

		private bool _mouseThrough;

		public bool MouseThrough
		{
			get => _mouseThrough;
			set
			{
				_mouseThrough = value;
				RaisePropertyChanged(() => MouseThrough);
			}
		}

		private bool _ninePattern;

		public bool NinePattern
		{
			get => _ninePattern;
			set
			{
				_ninePattern = value;
				RaisePropertyChanged(() => NinePattern);
			}
		}

	    private string _canvasScript;

	    public string CanvasScript
	    {
	        get => _canvasScript;
	        set
	        {
	            _canvasScript = value;
	            RaisePropertyChanged(() => CanvasScript);
	        }
	    }
	}
}