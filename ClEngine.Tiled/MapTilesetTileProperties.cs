namespace ClEngine.Tiled
{
	/// <remarks/>
	[System.Serializable]
	public class MapTilesetTileProperties
	{

		private MapTilesetTilePropertiesProperty _propertyField;

		/// <remarks/>
		public MapTilesetTilePropertiesProperty Property
		{
			get => _propertyField;
			set => _propertyField = value;
		}
	}
}