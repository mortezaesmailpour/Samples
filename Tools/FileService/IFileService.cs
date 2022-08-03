namespace Tools;
public interface IFileService
{
    IEnumerable<FileInfo> GetAllFiles(string path);
}