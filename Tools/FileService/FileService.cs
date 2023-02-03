using System.IO;
using Tools.Model;

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

    public async Task<bool> TrySaveToTextFileAsync(string path, string? contents)
    {
        try
        {
            if (File.Exists(path))
            {
                logger.Error("faild to save : the file is already exists in {0}", path);
                return false;
            }
            await File.WriteAllTextAsync(path, contents);
            logger.Info("file was saved in {0}", path);
            return true;
        }
        catch (Exception ex)
        {
            logger.Error("some thing went wrong : {0}", ex.Message);
            return false;
        }
    }
    public async Task<bool> TrySaveToDBAsync(FileModel file)
    {
        try
        {
            using (var context = new FileDbContext())
            {
                context.Files.Add(file);
                await context.SaveChangesAsync();
            }
            return true;
        }
        catch (Exception ex)
        {
            logger.Error("some thing went wrong : {0}", ex.Message);
            return false;
        }
    }
    public async Task<bool> TrySaveToDBAsync(List<FileModel> files)
    {
        foreach (var file in files)
            await TrySaveToDBAsync(file);
            try
        {
            using (var context = new FileDbContext())
            {
                foreach (var file in files)
                    context.Files.Add(file);
                await context.SaveChangesAsync();
            }
            return true;
        }
        catch (Exception ex)
        {
            logger.Error("some thing went wrong : {0}", ex.Message);
            return false;
        }
    }

    public static FileModel? GetFileModelFromFileInfo(FileInfo fileInfo)
    {
        if (fileInfo.Exists)
            return new FileModel()
            {
                Name = fileInfo.Name,
                Length = fileInfo.Length,
                Path = fileInfo.FullName,
                CreationTime = fileInfo.CreationTime,
                CreationTimeUtc = fileInfo.CreationTimeUtc,
                LastAccessTime = fileInfo.LastAccessTime,
                LastAccessTimeUtc = fileInfo.LastAccessTimeUtc,
                LastWriteTime = fileInfo.LastWriteTime,
                LastWriteTimeUtc = fileInfo.LastWriteTimeUtc,
                Attributes = fileInfo.Attributes,
                Description = fileInfo.IsReadOnly ? "It is readonly" : "It's not readonly"

            };
        return null;
    }
    public FileModel? GetFileInfo(string path)
    {
        try
        {
            var fileInfo = new FileInfo(path);
            return GetFileModelFromFileInfo(fileInfo);
        }
        catch (Exception ex)
        {
            logger.Error("some thing went wronge while getting FileInfo from {0} : {1}", path, ex.Message);
        }
        return null;
    }




    public void Merge(string sourceA, string sourceB, string destination)
    {
        var allFilesA = GetAllFiles(sourceA);
        var allFilesB = GetAllFiles(sourceB);
        foreach (var file in allFilesA)
        {
            var duplicates = allFilesB.Where(x => x.Name == file.Name && x.Length == file.Length && x.LastWriteTime== file.LastWriteTime).ToList();
            if (duplicates.Count > 0)
            {
                logger.Warning("{0} duplicates files was found :", duplicates.Count);
                foreach (var dup in duplicates)
                {
                    var dupFile = GetFileModelFromFileInfo(dup);
                    logger.Debug("{0}", dupFile?.Path);
                }
            }
        }
    }
}