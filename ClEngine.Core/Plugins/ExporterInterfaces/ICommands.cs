using ClEngine.Core.Plugins.ExporterInterfaces.CommandInterfaces;

namespace ClEngine.Core.Plugins.ExporterInterfaces
{
    public interface ICommands
    {
        IDialogCommands DialogCommands { get; }
    }
}