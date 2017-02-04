namespace FluentWindsor.Hawkeye.Interfaces
{
    public interface IFormatter
    {
        string Format(LoggingFormatterParams @params);
        bool IsSatisfiedBy(LogAttribute attribute);
    }
}