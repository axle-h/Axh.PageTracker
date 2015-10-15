namespace Axh.Core.Services.Logging.Contracts
{
    using System;

    public interface ILoggingService
    {
        /// <summary>
        /// Gets the name of the logger.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a value indicating whether messages with Debug severity should be logged.
        /// </summary>
        bool IsDebugEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether messages with Info severity should be logged.
        /// </summary>
        bool IsInfoEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether messages with Warn severity should be logged.
        /// </summary>
        bool IsWarnEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether messages with Error severity should be logged.
        /// </summary>
        bool IsErrorEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether messages with Fatal severity should be logged.
        /// </summary>
        bool IsFatalEnabled { get; }

        /// <summary>
        /// Logs the specified message with Debug severity.
        /// </summary>
        /// <param name="message">The message.</param>
        void Debug(string message);

        /// <summary>
        /// Logs the specified message with Debug severity.
        /// </summary>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        void Debug(string format, params object[] args);

        /// <summary>
        /// Logs the specified exception with Debug severity.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        void Debug(Exception exception, string format, params object[] args);

        /// <summary>
        /// Logs the specified exception with Debug severity.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception to log.</param>
        void DebugException(string message, Exception exception);

        /// <summary>
        /// Logs the specified message with Info severity.
        /// </summary>
        /// <param name="message">The message.</param>
        void Info(string message);

        /// <summary>
        /// Logs the specified message with Info severity.
        /// </summary>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        void Info(string format, params object[] args);

        /// <summary>
        /// Logs the specified exception with Info severity.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        void Info(Exception exception, string format, params object[] args);

        /// <summary>
        /// Logs the specified exception with Info severity.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception to log.</param>
        void InfoException(string message, Exception exception);

        /// <summary>
        /// Logs the specified message with Warn severity.
        /// </summary>
        /// <param name="message">The message.</param>
        void Warn(string message);

        /// <summary>
        /// Logs the specified message with Warn severity.
        /// </summary>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        void Warn(string format, params object[] args);

        /// <summary>
        /// Logs the specified exception with Warn severity.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        void Warn(Exception exception, string format, params object[] args);

        /// <summary>
        /// Logs the specified message with Warn severity.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception to log.</param>
        void WarnException(string message, Exception exception);

        /// <summary>
        /// Logs the specified message with Error severity.
        /// </summary>
        /// <param name="message">The message.</param>
        void Error(string message);

        /// <summary>
        /// Logs the specified message with Error severity.
        /// </summary>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        void Error(string format, params object[] args);

        /// <summary>
        /// Logs the specified exception with Error severity.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        void Error(Exception exception, string format, params object[] args);

        /// <summary>
        /// Logs the specified exception with Error severity.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception to log.</param>
        void ErrorException(string message, Exception exception);

        /// <summary>
        /// Logs the specified message with Fatal severity.
        /// </summary>
        /// <param name="message">The message.</param>
        void Fatal(string message);

        /// <summary>
        /// Logs the specified message with Fatal severity.
        /// </summary>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        void Fatal(string format, params object[] args);

        /// <summary>
        /// Logs the specified exception with Fatal severity.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        void Fatal(Exception exception, string format, params object[] args);

        /// <summary>
        /// Logs the specified exception with Fatal severity.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception to log.</param>
        void FatalException(string message, Exception exception);
    }
}
