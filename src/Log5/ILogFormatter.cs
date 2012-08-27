namespace Log5
{

    public interface ILogFormatter<out T>
    {
        T Format(LogEntry logEntry);
    }
}