using System.Xml.Serialization;

namespace ClEngine.Tiled
{
	[System.Serializable, XmlRoot(ElementName = "text")]
	public class MapText
	{
		private string _fontfamily;
		private string _pixelsize;
		private string _wrap;
		private string _color;
		private string _bold;
		private string _italic;
		private string _underline;
		private string _strikeout;
		private string _kerning;
		private string _halign;
		private string _valign;

		[XmlAttribute("valign")]
		public string Valign
		{
			get => _valign;
			set => _valign = value;
		}

		[XmlAttribute("halign")]
		public string Halign
		{
			get => _halign;
			set => _halign = value;
		}

		[XmlAttribute("kerning")]
		public string Kerning
		{
			get => _kerning;
			set => _kerning = value;
		}

		[XmlAttribute("strikeout")]
		public string Strikeout
		{
			get => _strikeout;
			set => _strikeout = value;
		}

		[XmlAttribute("underline")]
		public string Underline
		{
			get => _underline;
			set => _underline = value;
		}

		[XmlAttribute("italic")]
		public string Italic
		{
			get => _italic;
			set => _italic = value;
		}

		[XmlAttribute("fontfamily")]
		public string Fontfamily
		{
			get => _fontfamily;
			set => _fontfamily = value;
		}

		[XmlAttribute("pixelsize")]
		public string Pixelsize
		{
			get => _pixelsize;
			set => _pixelsize = value;
		}

		[XmlAttribute("wrap")]
		public string Wrap
		{
			get => _wrap;
			set => _wrap = value;
		}

		[XmlAttribute("color")]
		public string Color
		{
			get => _color;
			set => _color = value;
		}

		[XmlAttribute("bold")]
		public string Bold
		{
			get => _bold;
			set => _bold = value;
		}
	}
}