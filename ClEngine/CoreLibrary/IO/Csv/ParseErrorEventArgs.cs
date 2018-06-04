using System;

namespace ClEngine.CoreLibrary.IO.Csv
{
    public class ParseErrorEventArgs : EventArgs
    {
        public ParseErrorEventArgs(Exception error, ParseErrorAction defaultAction)
        {
            Error = error;
            Action = defaultAction;
        }

        public Exception Error { get; }

        public ParseErrorAction Action { get; set; }
    }
}