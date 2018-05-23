using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;

namespace ClEngine.CoreLibrary.Map
{
    public class CommonMapDraw : WpfGame
    {
        private IGraphicsDeviceService _graphicsDeviceService;

        protected override void Initialize()
        {
            _graphicsDeviceService = new WpfGraphicsDeviceService(this);

            base.Initialize();
        }
    }
}