using System.Collections.Generic;
using System.Xml.Serialization;

namespace ClEngine.Model
{
	public class MapModel
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

		public MapModel()
		{
			LogicGridWidth = 64;
			LogicGridHeight = 32;
			SurfaceGridWidth = 256;
			SurfaceGridHeight = 256;

			OpacityList = new List<List<int>>();
			BarrierList = new List<List<int>>();
			Block = new List<string>();
		}
	}
}