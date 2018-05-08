using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ClEngine.Particle.Modifiers;
using MonoGame.Extended.Serialization;

namespace ClEngine.Particle.Serialization
{
	public class ModifierJsonConverter : BaseTypeJsonConverter<Modifier>
	{
		public ModifierJsonConverter()
			: base(GetSupportedTypes(), "Modifier")
		{
		}

		private static IEnumerable<TypeInfo> GetSupportedTypes()
		{
			return typeof(Modifier)
				.GetTypeInfo()
				.Assembly
				.DefinedTypes
				.Where(type => typeof(Modifier).GetTypeInfo().IsAssignableFrom(type) && !type.IsAbstract);
		}
	}
}