namespace Axh.Core.Services.Logging
{
    using System;

    using Axh.Core.Services.Logging.Contracts;

    public class ConsoleLoggingService : ILoggingService
    {
        public string Name { get; } = "ConsoleLoggingService";

        public bool IsDebugEnabled { get; } = true;

        public bool IsInfoEnabled { get; } = true;

        public bool IsWarnEnabled { get; } = true;

        public bool IsErrorEnabled { get; } = true;

        public bool IsFatalEnabled { get; } = true;

        private static void Write(string message)
        {
            Console.WriteLine(message);
        }

        private static void Write(string format, object[] args)
        {
            Console.WriteLine(format, args);
        }

        private static void Write(Exception exception, string format, object[] args)
        {
            WriteException(string.Format(format, args), exception);
        }

        private static void WriteException(string message, Exception exception)
        {
            Console.WriteLine(message);
            Console.WriteLine(exception.Message);
            Console.WriteLine(exception.StackTrace);
        }

        public void Debug(string message)
        {
            Write(message);
        }

        public void Debug(string format, params object[] args)
        {
            Write(format, args);
        }

        public void Debug(Exception exception, string format, params object[] args)
        {
            Write(exception, format, args);
        }

        public void DebugException(string message, Exception exception)
        {
            WriteException(message, exception);
        }

        public void Info(string message)
        {
            Write(message);
        }

        public void Info(string format, params object[] args)
        {
            Write(format, args);
        }

        public void Info(Exception exception, string format, params object[] args)
        {
            Write(exception, format, args);
        }

        public void InfoException(string message, Exception exception)
        {
            WriteException(message, exception);
        }

        public void Warn(string message)
        {
            Write(message);
        }

        public void Warn(string format, params object[] args)
        {
            Write(format, args);
        }

        public void Warn(Exception exception, string format, params object[] args)
        {
            Write(exception, format, args);
        }

        public void WarnException(string message, Exception exception)
        {
            WriteException(message, exception);
        }

        public void Error(string message)
        {
            Write(message);
        }

        public void Error(string format, params object[] args)
        {
            Write(format, args);
        }

        public void Error(Exception exception, string format, params object[] args)
        {
            Write(exception, format, args);
        }

        public void ErrorException(string message, Exception exception)
        {
            WriteException(message, exception);
        }

        public void Fatal(string message)
        {
            Write(message);
        }

        public void Fatal(string format, params object[] args)
        {
            Write(format, args);
        }

        public void Fatal(Exception exception, string format, params object[] args)
        {
            Write(exception, format, args);
        }

        public void FatalException(string message, Exception exception)
        {
            WriteException(message, exception);
        }
    }
}
