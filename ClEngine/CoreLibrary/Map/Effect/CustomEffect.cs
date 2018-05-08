using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics.Effects;
using MonoGame.Extended.Tiled.Graphics.Effects;

namespace ClEngine.CoreLibrary.Map.Effect
{
	public class CustomEffect : DefaultEffect, ITiledMapEffect
	{
		public CustomEffect(GraphicsDevice graphicsDevice)
			: base(graphicsDevice)
		{
		}

		public CustomEffect(GraphicsDevice graphicsDevice, byte[] byteCode)
			: base(graphicsDevice, byteCode)
		{

		}

		public CustomEffect(Microsoft.Xna.Framework.Graphics.Effect cloneSource)
			: base(cloneSource)
		{
		}
	}
}