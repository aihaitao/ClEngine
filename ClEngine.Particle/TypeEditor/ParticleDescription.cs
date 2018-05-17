using System.ComponentModel;

namespace ClEngine.Particle.TypeEditor
{
	public class ParticleDescription : DescriptionAttribute
	{
		public string Des { get; }

		public override string Description => Des.GetTranslateName();

		public ParticleDescription(string description)
		{
			Des = description;
		}
	}
}