using FlatRedBall.Glue.Plugins.Interfaces;

namespace ClEngine.CoreLibrary.Plugins.Interfaces
{
    public interface IOutputReceiver : IPlugin
    {
        void OnOutput(string output);
        void OnErrorOutput(string output);
    }
}