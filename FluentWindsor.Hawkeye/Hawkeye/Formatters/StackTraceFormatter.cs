using System.Diagnostics;
using System.Text;
using FluentWindsor.Hawkeye.Interfaces;

namespace FluentWindsor.Hawkeye.Formatters
{
    public class StackTraceFormatter : IFormatter
    {
        private readonly MethodSignatureFormatter signatureFormatter = new MethodSignatureFormatter();

        public string Format(LoggingFormatterParams @params)
        {
            var builder = new StringBuilder();
            builder.AppendLine("Stacktrace");

            var stack = new StackTrace();

            foreach (var frame in stack.GetFrames())
            {
                builder.Append("\t");
                var method = frame.GetMethod();

                if (method.DeclaringType == null)
                    continue;

                builder.AppendLine(string.Format("{0}.{1}.{2}", method.DeclaringType.Namespace, method.DeclaringType.Name, signatureFormatter.GetSignature(method)));
            }

            return builder.ToString();
        }

        public bool IsSatisfiedBy(LogAttribute attribute)
        {
            return attribute.LogLevel == LogLevel.Debug;
        }
    }
}