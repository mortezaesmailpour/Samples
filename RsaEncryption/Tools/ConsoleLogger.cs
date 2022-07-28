namespace RsaEncryption.Tools;

internal class ConsoleLogger : ILogger
{
    private void Log(LogLevel logLevel, string message)
    {
        Console.ForegroundColor = logLevel switch
        {
            LogLevel.Error => ConsoleColor.Red,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Info => ConsoleColor.Green,
            LogLevel.Debug => ConsoleColor.White,
            _ => ConsoleColor.White,

        };
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public void LogInfo(string message) => Log(LogLevel.Info, message);
    public void LogDebug(string message) => Log(LogLevel.Debug, message);
    public void LogWarning(string message) => Log(LogLevel.Warning, message);
    public void LogError(string message) => Log(LogLevel.Error, message);
}
