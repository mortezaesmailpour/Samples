using Microsoft.Extensions.Logging;
namespace Tools;

public interface ILogger 
{
    void Info(string? message, params object?[] args);
    void Critical(string? message, params object?[] args);
    void Error(string? message, params object?[] args);
    void Debug(string? message, params object?[] args);
    void Warning(string? message, params object?[] args);
    void Trace(string? message, params object?[] args);
    void Test();
}