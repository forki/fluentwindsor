using System.Text;
using FluentlyWindsor.Hawkeye.Extensions;
using FluentlyWindsor.Hawkeye.Interfaces;

namespace FluentlyWindsor.Hawkeye.Formatters
{
    public class ExceptionFormatter : IFormatter
    {
        public string Format(LoggingFormatterParams @params)
        {
            var builder = new StringBuilder();
            if (@params.Exception != null)
                builder.WritePair("Exception", @params.Exception);
            return builder.ToString();
        }

        public bool IsSatisfiedBy(LogAttribute attribute)
        {
            return true;
        }
    }
}