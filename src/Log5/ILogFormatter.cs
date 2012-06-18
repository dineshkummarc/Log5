namespace Log5
{
    using System;

    public interface ILogFormatter
    {
        string Format(DateTime dateTime, LogLevel logLevel, string message);
    }
}