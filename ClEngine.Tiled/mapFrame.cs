using System;
using System.Xml.Serialization;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "frame")]
	public class MapFrame
	{
		private string _tileid;
		private TimeSpan _duration;

		[XmlAttribute("tileid")]
		public string Tileid
		{
			get => _tileid;
			set => _tileid = value;
		}

		[XmlElement(ElementName = "duration")]
		public TimeSpan Duration
		{
			get => _duration;
			set => _duration = value;
		}
	}
}