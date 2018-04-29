using System;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace ClEngine
{
	public class MyCompletionData : ICompletionData
	{
		public MyCompletionData(string text)
		{
			this.Text = text;
		}

		public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
		{
			textArea.Document.Replace(completionSegment, Text);
		}

		public ImageSource Image => null;
		public string Text { get; }
	
		public object Content => Text;
		public object Description => GetDescription(Text);
		public double Priority { get; }

		private string GetDescription(string text)
		{
			switch (text)
			{
				case "system":
					return "系统";
				case "system.width":
					return "屏幕宽度";
				case "system.height":
					return "屏幕高度";

				default:
					return "暂未描述";
			}
		}
	}
}