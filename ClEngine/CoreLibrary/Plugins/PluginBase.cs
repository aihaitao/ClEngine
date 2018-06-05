using System;
using FlatRedBall.Glue.Plugins.Interfaces;

namespace ClEngine.CoreLibrary.Plugins
{
    public abstract class PluginBase : IPlugin
    {
        public abstract string FriendlyName { get; }
        public abstract Version Version { get; }
        public abstract void StartUp();

        public abstract bool ShutDown(PluginShutDownReason shutDownReason);

        public OnErrorOutputDelegate OnErrorOutputHandler { get; protected set; }

        public Func<string, bool> CanFileReferenceContent { get; protected set; }
    }
}