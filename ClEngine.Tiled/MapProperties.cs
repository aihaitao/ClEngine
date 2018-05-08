namespace ClEngine.Tiled
{
	/// <remarks/>
	[System.Serializable]
	public class MapProperties
	{

		private MapPropertiesProperty _propertyField;

		/// <remarks/>
		public MapPropertiesProperty Property
		{
			get => _propertyField;
			set => _propertyField = value;
		}
	}
}