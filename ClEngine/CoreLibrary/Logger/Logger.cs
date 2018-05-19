using ClEngine.Model;
using GalaSoft.MvvmLight.Messaging;

namespace ClEngine.CoreLibrary.Logger
{
    public enum LogLevel
    {
        Log,
        Warn,
        Error,
    }

    public static class Logger
    {
        public static void Log(object message)
        {
            var logModel = new LogModel
            {
                Message = message,
                LogLevel = LogLevel.Log
            };

            Messenger.Default.Send(logModel, "Log");
        }

        public static void Warn(object message)
        {
            var logModel = new LogModel
            {
                Message = message,
                LogLevel = LogLevel.Warn
            };
            Messenger.Default.Send(logModel, "Log");
        }

        public static void Error(object message)
        {
            var logModel = new LogModel
            {
                Message = message,
                LogLevel = LogLevel.Error
            };
            Messenger.Default.Send(logModel, "Log");
        }
    }
}