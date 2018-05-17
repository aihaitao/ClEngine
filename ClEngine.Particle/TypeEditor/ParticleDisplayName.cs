using System.ComponentModel;

namespace ClEngine.Particle.TypeEditor
{
	public class ParticleDisplayName : DisplayNameAttribute
	{
		public string Name { get; }
		public override string DisplayName => Name.GetTranslateName();

		public ParticleDisplayName(string name)
		{
			Name = name;
		}
	}
}