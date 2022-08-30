using Tools.Model;

namespace Tools;
public interface IFileService
{
    IEnumerable<FileInfo> GetAllFiles(string path);
    FileModel? GetFileInfo(string path);
    Task<bool> TrySaveToTextFileAsync(string path, string? contents);
}