using System.Diagnostics;
using System.IO;
using System.Threading;
using ClEngine.CoreLibrary.Editor;

namespace ClEngine.CoreLibrary.Asset
{
	public class AssetTranslator
	{
		private static AssetTranslator _instance;
		private string TranslatorName => "MGCB.exe";
		private string TranslatorEnvironment => Path.Combine(EditorRecord.EditorEnvironment, "runtime", "assetcompiler");
		private string TranslatorFullName => Path.Combine(TranslatorEnvironment, TranslatorName);

		public static AssetTranslator GetTranslator()
		{
			if (_instance == null)
				return new AssetTranslator();

			return _instance;
		}

		public void Compiler(string arguments)
		{
			ThreadPool.QueueUserWorkItem(delegate
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
				Process.Start(startInfo);
			});
		}
	}
}