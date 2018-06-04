using FlatRedBall.Glue.Plugins;
using RenderingLibrary;

namespace ClEngine.CoreLibrary.Plugins
{
    public class PluginManager : PluginManagerBase
    {
        public PluginManager(bool global) : base(global)
        {
            IPositionedSizedObject test = null;

            var throwaway = ToolsUtilities.FileManager.GetExtension("something.png");
            var anotherThrowaway = typeof(XnaAndWinforms.GraphicsDeviceControl);
            var throwaway2 = typeof(FlatRedBall.SpecializedXnaControls.TimeManager);
            var throwaway3 = typeof(FlatRedBall.SpecializedXnaControls.Input.CameraPanningLogic);

            var throwaway4 = typeof(InputLibrary.Keyboard);
            var throwaway5 = typeof(RenderingLibrary.Camera);
            var throwaway6 = typeof(ToolsUtilities.FileManager);
            var throwaway7 = typeof(XnaAndWinforms.GraphicsDeviceControl);
        }
    }
}