using System;
using System.IO;
using System.Reflection;
using System.Resources;

namespace ClEngine.Particle
{
	public static class ParticleHelper
	{
		private static readonly Assembly CurrentEntryAssembly =
			Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "ClEngine.Particle.dll"));
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

			var resourceManager = new ResourceManager("ClEngine.Particle.Properties.Resources", CurrentEntryAssembly);
			var translateName = resourceManager.GetObject(name);
			return translateName != null ? translateName.ToString() : name;
		}
	}
}