using System;
using System.Windows.Forms;

namespace ClEngine.CoreLibrary.Editor
{
	public static class EditorRecord
	{
		public static string EditorEnvironment => Application.StartupPath;
	    public static string GameEnvironment => Environment.CurrentDirectory;
	}
}