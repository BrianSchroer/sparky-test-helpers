using Microsoft.Web.XmlTransform;
using System;
using System.Text;

namespace SparkyTestHelpers.Xml.Transformation
{
    internal class TransformationLogger : IXmlTransformationLogger
    {
        private readonly bool _verbose;
        private readonly StringBuilder _stringBuilder;
        private string _prefix = null;

        public TransformationLogger(StringBuilder stringBuilder, bool verbose = true)
        {
            _stringBuilder = stringBuilder;
            _verbose = verbose;
        }

        bool ShouldPrint(MessageType type)
            => type == MessageType.Normal || _verbose;

        public void LogMessage(string message, params object[] messageArgs)
            => Log(message, messageArgs);

        public void LogMessage(MessageType type, string message, params object[] messageArgs)
        {
            if (ShouldPrint(type)) Log(message, messageArgs);
        }

        public void LogWarning(string message, params object[] messageArgs)
            => Log($"WARN: {message}", messageArgs);

        public void LogWarning(string file, string message, params object[] messageArgs)
            => Log($"WARN '{file}': {message}", messageArgs);

        public void LogWarning(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
            => Log($"WARN '{file}':{lineNumber}:{linePosition}: {message}", messageArgs);

        public void LogError(string message, params object[] messageArgs)
            => Log($"ERROR: {message}", messageArgs);

        public void LogError(string file, string message, params object[] messageArgs)
            => Log($"ERROR '{file}': {message}", messageArgs);

        public void LogError(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
            => Log($"ERROR '{file}':{lineNumber}:{linePosition}: {message}", messageArgs);

        public void LogErrorFromException(Exception ex)
            => Log($"ERROR: {ex}");

        public void LogErrorFromException(Exception ex, string file)
            => Log($"ERROR '{file}': {ex}");

        public void LogErrorFromException(Exception ex, string file, int lineNumber, int linePosition)
            => Log($"ERROR '{file}':{lineNumber}:{linePosition}: {ex}");

        public void StartSection(string message, params object[] messageArgs)
        {
            Log($"Start {message}", messageArgs);
            _prefix = "\t";
        }

        public void StartSection(MessageType type, string message, params object[] messageArgs)
        {
            if (ShouldPrint(type))
            {
                Log($"Start {message}", messageArgs);
                _prefix = "\t";
            }
        }

        public void EndSection(string message, params object[] messageArgs)
        {
            _prefix = null;
            Log($"End {message}", messageArgs);
        }

        public void EndSection(MessageType type, string message, params object[] messageArgs)
        {
            if (ShouldPrint(type))
            {
                _prefix = null;
                Log($"End {message}", messageArgs);
            }
        }

        private void Log(string format, params object[] args)
        {
            _stringBuilder.AppendLine(string.Format($"{_prefix}{format}", args));
        }
    }
}