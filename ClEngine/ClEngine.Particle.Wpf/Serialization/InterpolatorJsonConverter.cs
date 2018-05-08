using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ClEngine.Particle.Modifiers;
using MonoGame.Extended.Serialization;

namespace ClEngine.Particle.Serialization
{
	public class InterpolatorJsonConverter : BaseTypeJsonConverter<Interpolator>
	{
		public InterpolatorJsonConverter()
			: base(GetSupportedTypes(), "Interpolator")
		{
		}

		private static IEnumerable<TypeInfo> GetSupportedTypes()
		{
			return typeof(Interpolator)
				.GetTypeInfo()
				.Assembly
				.DefinedTypes
				.Where(type => typeof(Interpolator).GetTypeInfo().IsAssignableFrom(type) && !type.IsAbstract);
		}
	}
}