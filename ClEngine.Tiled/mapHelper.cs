using System.IO;
using System.Xml.Serialization;

namespace ClEngine.Tiled
{
	public static class MapHelper
	{
		public static Map GetMap(string path)
		{
			using (var fileStream = new FileStream(path, FileMode.Open))
			{
				var serializer = new XmlSerializer(typeof(Map));
				return (Map) serializer.Deserialize(fileStream);
			}
		}
	}
}