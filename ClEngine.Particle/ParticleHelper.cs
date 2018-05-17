using System.Resources;
using ClEngine.Particle.Properties;

namespace ClEngine.Particle
{
	public static class ParticleHelper
	{
		public static dynamic GetTranslateName(this object obj)
		{
			if (obj is string name)
			{
				return BeginTranslateName(name);
			}

			return obj;
		}

		private static string BeginTranslateName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				return string.Empty;

			var resourceManager = new ResourceManager("ClEngine.Particle.Properties.Resources", typeof(Resources).Assembly);
			var translateName = resourceManager.GetObject(name);
			return translateName != null ? translateName.ToString() : name;
		}
	}
}