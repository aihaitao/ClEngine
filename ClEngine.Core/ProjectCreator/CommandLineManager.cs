using System;

namespace ClEngine.Core.ProjectCreator
{
    public class CommandLineManager : Singleton<CommandLineManager>
    {
        public string ProjectLocation { get; private set; }

        public string DifferentNamespace { get; private set; }

        public string OpenedBy { get; private set; }

        public bool EmptyProjectsOnly { get; private set; }

        public void ProcessCommandLineArguments()
        {
            foreach (var commandLineArg in Environment.GetCommandLineArgs())
            {
                if (commandLineArg.StartsWith("directory="))
                    HandleDirectoryEquals(commandLineArg);
                else if (commandLineArg.StartsWith("namespace="))
                    HandleNamespaceEquals(commandLineArg);
                else if (commandLineArg.ToLower().StartsWith("openedby="))
                    HandleOpenedBy(commandLineArg);
                else if (commandLineArg.ToLower() == "emptyprojects")
                    EmptyProjectsOnly = true;
            }
        }

        private void HandleDirectoryEquals(string arg)
        {
            var lengthOfDirectory = "directory=".Length;

            var directory = arg.Substring(lengthOfDirectory, arg.Length - lengthOfDirectory);

            if (directory.StartsWith("\"") && directory.EndsWith("\""))
                directory = directory.Substring(1, directory.Length - 2);

            directory = directory.Replace("/", "\\");

            ProjectLocation = directory;
        }

        private void HandleNamespaceEquals(string args)
        {
            var lengthOfNamespaceConstant = "namespace=".Length;
            var value = args.Substring(lengthOfNamespaceConstant, args.Length - lengthOfNamespaceConstant);

            DifferentNamespace = value;
        }

        private void HandleOpenedBy(string arg)
        {
            var indexOfEquals = arg.IndexOf('=');

            var value = arg.Substring(indexOfEquals + 1);

            OpenedBy = value;
        }
    }
}