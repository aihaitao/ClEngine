namespace ClEngine.Tiled
{
	/// <remarks/>
	[System.Serializable]
	public class MapPropertiesProperty
	{

		private string _nameField;

		private string _valueField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute]
		public string Name
		{
			get => _nameField;
			set => _nameField = value;
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttribute]
		public string Value
		{
			get => _valueField;
			set => _valueField = value;
		}
	}
}