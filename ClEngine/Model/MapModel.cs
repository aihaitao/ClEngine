using System.Collections.Generic;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;

namespace ClEngine.Model
{
	public class MapModel : ObservableObject
	{
		[XmlElement("地图名称")]
		public string Name { get; set; }

		[XmlElement("图片名称")]
		public string ImageName { get; set; }

		[XmlElement("障碍")]
		public List<List<int>> BarrierList { get; set; }

		[XmlElement("透明")]
		public List<List<int>> OpacityList { get; set; }

		[XmlElement("区块")]
		public List<string> Block { get; set; }

		[XmlElement("逻辑格子宽度")]
		public int LogicGridWidth { get; set; }

		[XmlElement("逻辑格子高度")]
		public int LogicGridHeight { get; set; }

		[XmlElement("地表格子宽度")]
		public int SurfaceGridWidth { get; set; }

		[XmlElement("地表格子高度")]
		public int SurfaceGridHeight { get; set; }

		private int _blockWidth;

		public int BlockWidth
		{
			get => _blockWidth;
			set
			{
				_blockWidth = value;
				RaisePropertyChanged(() => BlockWidth);
			}
		}

		private int _blockHeight;

		public int BlockHeight
		{
			get => _blockHeight;
			set
			{
				_blockHeight = value;
				RaisePropertyChanged(() => _blockHeight);
			}
		}

		private int _fixedWidth;

		public int FixedWidth
		{
			get => _fixedWidth;
			set
			{
				_fixedWidth = value;
				RaisePropertyChanged(() => FixedWidth);
			}
		}

		private int _fixedHeight;

		public int FixedHeight
		{
			get => _fixedHeight;
			set
			{
				_fixedHeight = value;
				RaisePropertyChanged(() => FixedHeight);
			}
		}

		public MapModel()
		{
			LogicGridWidth = 64;
			LogicGridHeight = 32;
			SurfaceGridWidth = 256;
			SurfaceGridHeight = 256;
			BlockWidth = 32;
			BlockHeight = 32;
			FixedWidth = 100;
			FixedHeight = 100;

			OpacityList = new List<List<int>>();
			BarrierList = new List<List<int>>();
			Block = new List<string>();
		}
	}
}