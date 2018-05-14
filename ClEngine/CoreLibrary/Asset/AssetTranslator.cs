using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Xml.Serialization;

namespace ClEngine.CoreLibrary.Asset
{
	public class AssetTranslator
	{
		private static AssetTranslator _instance;
		public string Arguments { get; set; }
		private string TranslatorName => "MGCB.exe";
		private string TranslatorEnvironment => Path.Combine(EditorRecord.EditorEnvironment, "runtime", "assetcompiler");
		private string TranslatorFullName => Path.Combine(TranslatorEnvironment, TranslatorName);
		private string Intermediate = Path.Combine(MainWindow.ProjectPosition, "Intermediate");

		public static AssetTranslator GetTranslator()
		{
			if (_instance == null)
				return new AssetTranslator();

			return _instance;
		}

		public void Compiler()
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				var startInfo = new ProcessStartInfo(TranslatorFullName)
				{
					CreateNoWindow = true,
					WorkingDirectory = TranslatorEnvironment,
					RedirectStandardError = true,
					RedirectStandardOutput = true,
					Arguments = Arguments,
					UseShellExecute = false,
				};
				Process.Start(startInfo);
			});
		}

		private SourceFileCollection GetMgContent()
		{
			var intermediateContent = Path.Combine(Intermediate, ".mgcontent");
			var sourceFileCollection = new SourceFileCollection();
			if (File.Exists(intermediateContent))
			{
				using (var stream = new FileStream(intermediateContent, FileMode.Open))
				{
					var xmldes = new XmlSerializer(typeof(SourceFileCollection));
					sourceFileCollection = (SourceFileCollection) xmldes.Deserialize(stream);
				}
			}

			return sourceFileCollection;
		}
	}
}