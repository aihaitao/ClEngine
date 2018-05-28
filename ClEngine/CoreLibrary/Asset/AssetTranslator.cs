using System;
using System.Diagnostics;
using System.IO;
using ClEngine.CoreLibrary.Editor;

namespace ClEngine.CoreLibrary.Asset
{
    public class AssetTranslator
	{
		private static AssetTranslator _instance;
		private string TranslatorName => "MGCB.exe";
		private string TranslatorEnvironment => Path.Combine(EditorRecord.EditorEnvironment, "runtime", "assetcompiler");
		private string TranslatorFullName => Path.Combine(TranslatorEnvironment, TranslatorName);

	    public Action Complete;

		public static AssetTranslator GetTranslator()
		{
			if (_instance == null)
				return new AssetTranslator();

			return _instance;
		}

		public void Compiler(string arguments)
		{
			var startInfo = new ProcessStartInfo(TranslatorFullName)
			{
				CreateNoWindow = true,
				WorkingDirectory = TranslatorEnvironment,
				RedirectStandardError = true,
				RedirectStandardOutput = true,
				Arguments = arguments,
				UseShellExecute = false,
			};
			var process = Process.Start(startInfo);
		    if (process != null)
		    {
		        process.EnableRaisingEvents = true;
		        process.Exited += (sender, args) => Complete?.Invoke();
            }
		}
	}
}