using System.Text;
using FluentlyWindsor.Hawkeye.Extensions;
using FluentlyWindsor.Hawkeye.Interfaces;

namespace FluentlyWindsor.Hawkeye.Formatters
{
    public class MethodCallFormatter : IFormatter
    {
        private readonly MethodSignatureFormatter signatureFormatter = new MethodSignatureFormatter();

        public string Format(LoggingFormatterParams @params)
        {
            var builder = new StringBuilder();
            
            builder.WritePair("Type", @params.Invocation.TargetType.Name);
            builder.WritePair("Method", signatureFormatter.GetSignature(@params.Invocation.MethodInvocationTarget, @params.Invocation.Arguments));

            if (@params.Invocation.ReturnValue != null)
                builder.WritePair("Return", @params.Invocation.ReturnValue);

            builder.WritePair("Executed", string.Format("{0} millisecond(s)", @params.ElapsedExecution.TotalMilliseconds));
            
            return builder.ToString();
        }

        public bool IsSatisfiedBy(LogAttribute attribute)
        {
            return true;
        }
    }
}
