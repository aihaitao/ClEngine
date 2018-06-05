using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClEngine.CoreLibrary.Plugins.ExportedImplementations;
using ClEngine.CoreLibrary.Plugins.Interfaces;
using FlatRedBall.Glue.Plugins;
using FlatRedBall.Glue.Plugins.Interfaces;
using FlatRedBall.IO;
using RenderingLibrary;

namespace ClEngine.CoreLibrary.Plugins
{
    public class PluginManager : PluginManagerBase
    {
        [ImportMany(AllowRecomposition = true)]
        public IEnumerable<IOutputReceiver> OutputReceiverPlugins { get; set; }

        [ImportMany(AllowRecomposition = true)]
        public IEnumerable<PluginBase> ImportedPlugins { get; set; }

        static StringBuilder mPreInitializeError = new StringBuilder();

        private static MenuStrip mMenuStrip;
        private static bool mHandleExceptions = true;
        public static bool HandleExceptions
        {
            get { return mHandleExceptions; }
            set { mHandleExceptions = value; }
        }

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

        static void CallMethodOnPluginNotUiThread(Action<PluginBase> methodToCall, string methodName)
        {
            var instances = mInstances.ToList();
            foreach (PluginManager manager in instances)
            {
                foreach (var plugin in manager.PluginContainers.Keys.Where(plugin => plugin is PluginBase))
                {
                    PluginContainer container = manager.PluginContainers[plugin];

                    if (container.IsEnabled)
                    {
                        IPlugin plugin1 = plugin;
                        PluginCommandNotUiThread(() =>
                            {
                                methodToCall(plugin1 as PluginBase);
                            }, container, "Failed in " + methodName);
                    }
                }
            }

        }

        private static void PluginCommandNotUiThread(Action action, PluginContainer container, string message)
        {
            if (HandleExceptions)
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    container.Fail(e, message);

                    ReceiveError(message + "\n" + e.ToString());
                }
            }
            else
            {
                action();
            }
        }

        public static void ReceiveError(string output)
        {
            if (!string.IsNullOrEmpty(output))
            {
                output = System.DateTime.Now.ToLongTimeString() + " - " + output;

                if (mInstances == null || mInstances.Count == 0)
                {
                    mPreInitializeError.AppendLine(output);
                }
                else
                {
                    var instances = mInstances.ToList();

                    foreach (PluginManager pluginManager in instances)
                    {
                        PrintError(output, pluginManager);
                    }
                }
            }
        }

        private static void PluginCommand(Action action, PluginContainer container, string message)
        {
            if (HandleExceptions)
            {
                if (mMenuStrip.IsDisposed)
                {
                    try
                    {
                        action();
                    }
                    catch (Exception e)
                    {
                        var version = container.Plugin.Version;

                        message = $"{container.Name} Version {version} {message}";

                        container.Fail(e, message);

                        ReceiveError(message + "\n" + e.ToString());


                    }
                }
                else
                {
                    // Do this on a UI thread
                    mMenuStrip.Invoke((MethodInvoker)delegate
                    {
                        try
                        {
                            action();
                        }
                        catch (Exception e)
                        {
                            var version = container.Plugin.Version;

                            message = $"{container.Name} Version {version} {message}";

                            container.Fail(e, message);

                            ReceiveError(message + "\n" + e.ToString());


                        }
                    });
                }
            }
            else
            {
                action();
            }
        }

        private static void PrintError(string output, PluginManager pluginManager)
        {
            foreach (IOutputReceiver plugin in pluginManager.OutputReceiverPlugins)
            {
                PluginContainer container = pluginManager.mPluginContainers[plugin];

                if (container.IsEnabled)
                {
                    IOutputReceiver plugin1 = plugin;
                    PluginCommand(() =>
                    {
                        plugin1.OnErrorOutput(output);
                    }, container, "Failed in ReactToChangedFile");
                }
            }

            // Execute the new style plugins
            var plugins = pluginManager.ImportedPlugins.Where(x => x.OnErrorOutputHandler != null);
            foreach (var plugin in plugins)
            {
                var container = pluginManager.mPluginContainers[plugin];
                if (container.IsEnabled)
                {
                    PluginBase plugin1 = plugin;
                    PluginCommand(() =>
                    {
                        plugin1.OnErrorOutputHandler(output);
                    }, container, "Failed in OnErrorOutput");
                }
            }
        }

        internal static bool CanFileReferenceContent(string absoluteName)
        {

            SaveRelativeDirectory();

            bool toReturn = false;


            CallMethodOnPluginNotUiThread(
                delegate (PluginBase plugin)
                {
                    if (plugin.CanFileReferenceContent != null)
                    {
                        toReturn |= plugin.CanFileReferenceContent(absoluteName);

                    }
                },
                "CanFileReferenceContent");

            ResumeRelativeDirectory("CanFileReferenceContent");
            return toReturn;


        }

        static void ResumeRelativeDirectory(string function)
        {
            if (mOldRelativeDirectories.Count == 0)
            {
                FileManager.RelativeDirectory = FileManager.GetDirectory(
                    GlueState.Self.CurrentGlueProjectFileName);
            }
            else
            {
                bool differs = true;

                try
                {
                    string top = null;
                    if (mOldRelativeDirectories.TryPeek(out top))
                    {
                        differs = FileManager.RelativeDirectory != top;
                    }
                }
                catch
                {
                    // no big deal we'll just act as if it differs
                }
                if (differs)
                {
                    ReceiveError("The relativeDirectory wasn't set properly in " + function);

                    string top = null;
                    if (mOldRelativeDirectories.TryPeek(out top))
                    {
                        FileManager.RelativeDirectory = top;
                    }

                }

                try
                {
                    string throwaway;
                    mOldRelativeDirectories.TryPop(out throwaway);
                }
                catch (ArgumentOutOfRangeException)
                {
                    // no big deal
                }
            }
        }

        static System.Collections.Concurrent.ConcurrentStack<string> mOldRelativeDirectories = new System.Collections.Concurrent.ConcurrentStack<string>();

        static void SaveRelativeDirectory()
        {

            mOldRelativeDirectories.Push(FileManager.RelativeDirectory);
        }
    }
}