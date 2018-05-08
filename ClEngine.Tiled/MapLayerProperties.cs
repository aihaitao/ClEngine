namespace ClEngine.Tiled
{
	/// <remarks/>
	[System.Serializable]
	public class MapLayerProperties
	{

		private MapLayerPropertiesProperty _propertyField;

		/// <remarks/>
		public MapLayerPropertiesProperty Property
		{
			get => _propertyField;
			set => _propertyField = value;
		}
	}
}