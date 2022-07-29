using Microsoft.Extensions.Logging;

namespace Tools;

public class ConsoleLogger : ILogger
{
    public void Test()
    {
        Console.WriteLine("Logs are Printing ...");
        Critical("Log Critical");
        Error("Log Error");
        Warning("Log Warning");
        Debug("Log Debug");
        Trace("Log Trace");
        Info("Log Information");
        Console.WriteLine("-------------------------------");
    }
    private static void Log(LogLevel logLevel, string? message, params object?[] args)
    {
        Console.ForegroundColor = logLevel switch
        {
            LogLevel.Critical => ConsoleColor.DarkRed,
            LogLevel.Error => ConsoleColor.Red,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Debug => ConsoleColor.Blue,
            LogLevel.Trace => ConsoleColor.Magenta,
            LogLevel.Information => ConsoleColor.DarkGreen,
            _ => ConsoleColor.White,

        };
        Console.WriteLine(message,args);
        Console.ResetColor();
    }

    public void Info(string? message, params object?[] args) => Log(LogLevel.Information ,  message, args);
    public void Critical(string? message, params object?[] args) => Log(LogLevel.Critical, message, args);
    public void Error(string? message, params object?[] args) => Log(LogLevel.Error, message, args);
    public void Debug(string? message, params object?[] args) => Log(LogLevel.Debug, message, args);
    public void Warning(string? message, params object?[] args) => Log(LogLevel.Warning, message, args);
    public void Trace(string? message, params object?[] args) => Log(LogLevel.Trace, message, args);

}
