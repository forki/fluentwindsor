using System.Linq;
using System.Text;
using FluentlyWindsor.Hawkeye.Formatters;
using FluentlyWindsor.Hawkeye.Interfaces;

namespace FluentlyWindsor.Hawkeye
{
    public class AggregateLoggingFormatter : ILoggingFormatter
    {
        private readonly IFormatter[] formatters;

        public AggregateLoggingFormatter()
        {
            formatters = new[] { new MethodCallFormatter() };
        }

        public AggregateLoggingFormatter(IFormatter[] formatters)
        {
            this.formatters = formatters;
        }

        public virtual string GetLogFormat(LoggingFormatterParams @params, LogAttribute attribute)
        {
            var builder = new StringBuilder();

            foreach (var formatter in formatters.Where(x => x.IsSatisfiedBy(attribute)))
                builder.Append(formatter.Format(@params));

            return builder.ToString();
        }
    }
}