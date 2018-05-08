using Microsoft.Xna.Framework;

namespace ClEngine.Particle.Profiles
{
	public class PointProfile : Profile
	{
		public override void GetOffsetAndHeading(out Vector2 offset, out Vector2 heading)
		{
			offset = Vector2.One;
			
			Random.NextUnitVector(out heading);
		}
	}
}