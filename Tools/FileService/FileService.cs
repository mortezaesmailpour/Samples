using System.Runtime;
using System.Linq;

namespace Tools;

public class FileService : IFileService
{
    private readonly ILogger logger = new ConsoleLogger();

    public IEnumerable<FileInfo> GetAllFiles(string path)
    {
        List<FileInfo> files = new List<FileInfo>();
        GetAllFiles(new DirectoryInfo(path), ref files);
        return files;
    }

    private void GetAllFiles(DirectoryInfo dirInfo, ref List<FileInfo> fileInfos)
    {
        fileInfos.AddRange(from file in GetFiles(dirInfo) select file);
        foreach (var directory in GetDirectories(dirInfo))
            GetAllFiles(directory, ref fileInfos);
    }
    private FileInfo[] GetFiles(DirectoryInfo dirInfo)
    {
        try
        {
            return dirInfo.GetFiles();
        }
        catch (Exception ex)
        {
            logger.Error("while GetFiles({0}) some thing went wronge : {1}",
                dirInfo.FullName, ex.Message);
        }
        return new FileInfo[0];
    }
    private DirectoryInfo[] GetDirectories(DirectoryInfo dirInfo)
    {
        try
        {
            return dirInfo.GetDirectories();
        }
        catch (Exception ex)
        {
            logger.Error("while GetDirectories({0}) some thing went wronge : {1}",
                dirInfo.FullName, ex.Message);
        }
        return new DirectoryInfo[0];
    }

    //JFT
    public IEnumerable<FileInfo> GetAllFiles_WY(string path) { return GetAllFiles_WY(new DirectoryInfo(path)); }
    private IEnumerable<FileInfo> GetAllFiles_WY(DirectoryInfo dirInfo)
    {
        FileInfo[]? files = null;
        try { files = dirInfo.GetFiles(); } catch (Exception ex) { logger.Error("while GetFiles({0}) some thing went wronge : {1}", dirInfo.FullName, ex.Message); }
        if (files is not null)
            foreach (var file in files)
                yield return file;

        DirectoryInfo[]? directories = null;
        try { directories = dirInfo.GetDirectories(); } catch (Exception ex) { logger.Error("while GetDirectories({0}) some thing went wronge : {1}", dirInfo.FullName, ex.Message); }
        if (directories is not null)
            foreach (var directory in directories)
                foreach (var file in GetAllFiles_WY(directory))
                    yield return file;
    }
}