namespace FluentlyWindsor.Hawkeye.Interfaces
{
    public interface ILoggingFormatter
    {
        string GetLogFormat(LoggingFormatterParams @params, LogAttribute attribute);
    }
}