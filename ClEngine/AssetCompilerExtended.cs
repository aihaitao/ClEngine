using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using ClEngine.Model;
using ClEngine.View.Map;
using GalaSoft.MvvmLight.Messaging;

namespace ClEngine
{
	public static class AssetCompilerExtended
	{
		private const string ResourceCompiler = "MGCB.exe";
		private static string AssetPath { get; set; }
		public static void Compiler(this string assetPath)
		{
			AssetPath = assetPath;
			var resourceCompilerDir = Path.Combine(Environment.CurrentDirectory, "runtime", "assetcompiler");
			var resourceCompilerPath = Path.Combine(resourceCompilerDir, ResourceCompiler);
			var arguments = GetArguments(assetPath);
			var startInfo = new ProcessStartInfo(resourceCompilerPath)
			{
				CreateNoWindow = true,
				WorkingDirectory = resourceCompilerDir,
				RedirectStandardError = true,
				RedirectStandardOutput = true,
				Arguments = arguments,
				UseShellExecute = false,
			};
			var process = Process.Start(startInfo);
			if (process != null)
			{
				process.EnableRaisingEvents = true;

				process.ErrorDataReceived += ProcessOnErrorDataReceived;
				process.OutputDataReceived += ProcessOnOutputDataReceived;
				process.Exited += ProcessOnExited;
				process.BeginErrorReadLine();
				process.BeginOutputReadLine();
			}
		}

		private static void ProcessOnExited(object sender, EventArgs e)
		{
			if (File.Exists(AssetPath))
				File.Delete(AssetPath);
		}

		private static string GetArguments(string assetPath)
		{
			var arguments = string.Empty;
			arguments += string.Concat(" /build:", assetPath);
			arguments += string.Concat(" /intermediateDir:", Path.Combine(MainWindow.ProjectPosition, "Intermediate"));
			arguments += string.Concat(" /clean:", "false");
			arguments += string.Concat(" /rebuild:", "false");
			arguments += string.Concat(" /compress:", "true");
			return arguments;
		}

		private static void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			var logModel = new LogModel
			{
				Message = e.Data,
				LogLevel = LogLevel.Log
			};
			Messenger.Default.Send(logModel, "Log");
		}

		private static void ProcessOnErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			var logModel = new LogModel
			{
				Message = e.Data,
				LogLevel = LogLevel.Error
			};
			Messenger.Default.Send(logModel, "Log");
		}
	}
}