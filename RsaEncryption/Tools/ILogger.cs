namespace RsaEncryption.Tools;

internal interface ILogger
{
    public void LogInfo(string message);
    public void LogDebug(string message);
    public void LogWarning(string message);
    public void LogError(string message);
}