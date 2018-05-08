using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ClEngine.Particle.Profiles;
using MonoGame.Extended.Serialization;

namespace ClEngine.Particle.Serialization
{
	public class ProfileJsonConverter : BaseTypeJsonConverter<Profile>
	{
		public ProfileJsonConverter()
			: base(GetSupportedTypes(), nameof(Profile))
		{
		}

		private static IEnumerable<TypeInfo> GetSupportedTypes()
		{
			return typeof(Profile)
				.GetTypeInfo()
				.Assembly
				.DefinedTypes
				.Where(type => type.IsSubclassOf(typeof(Profile)) && !type.IsAbstract);
		}
	}
}