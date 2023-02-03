using RsaEncryption;
using System.Runtime;
using Tools.Model;
using Tools;

ILogger logger = new ConsoleLogger();

logger.Test();


//--------------

FileServiceDemo(@"C:\morteza");






Console.WriteLine("Press any key to Continue.");
Console.ReadKey();
//--------------

void FileServiceDemo(string Path)
{
    List<string> list = new();
    var stopWatch = Stopwatch.StartNew();
    logger.Trace("FileService Demo is starting ...");
    try
    {
        var fileService = new Tools.FileService();
        var files = fileService.GetAllFiles(Path).ToList();
        var largeFile = files.Where(f => f.Length > 100000000).ToList();
        var sortedFiles = largeFile.OrderBy(f => f.Length).ToList();

        List<FileModel> fileModels = new();
        
        foreach (var file in files)
            fileModels.Add(Tools.FileService.GetFileModelFromFileInfo(file));

        fileService.TrySaveToDBAsync(fileModels);
        //fileService.TrySaveToDBAsync(Tools.FileService.GetFileModelFromFileInfo(sortedFiles.First()));
        fileService.TrySaveToTextFileAsync(@"C:\\Morteza\\test.txt",
            String.Join("\n", from file in files select Tools.FileService.GetFileModelFromFileInfo(file)));
        foreach (var file in sortedFiles)
        {
            //list.Add(file.FullName+"!");
            Console.WriteLine(file.FullName);
        }
        logger.Trace("{0} files was found.", files.Count);
    }
    catch (Exception ex) { logger.Error("something went wrong : " + ex.Message); }
    finally
    {
        stopWatch.Stop();
        logger.Trace("RSA FileService Demo was finished in " + stopWatch.ElapsedMilliseconds + "ms");
    }
}
//--------------


RSAEncryptionDemo(GetRandomString(1001007));


Console.WriteLine("Press any key to exit.");
Console.ReadKey();
//--------------

string GetRandomString(int length)
{
    Random rand = new Random();
    string str100 = "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
    string str = "";
    for (int i = 0; i < length % 100; i++)
        str += Convert.ToChar(rand.Next(0, 26) + 65);
    StringBuilder sb = new StringBuilder();
    for (int i = 0; i < length / 100; i++)
        sb.Append(str100);

    string result = sb.ToString();
    return result + str;
}

void RSAEncryptionDemo(string msg)
{
    var stopWatch = Stopwatch.StartNew();
    logger.Trace("RSA Encryption Demo is starting ...");
    try
    {
        IRsaService rsaService = new RsaService();
        (string publicKey, string privateKey) = rsaService.Create();
        logger.Info("publicKey : " + publicKey);
        //logger.Info("privateKey : " + privateKey);
        //logger.LogDebug("msg : " + msg);
        logger.Debug("msg size : " + msg.Length);
        var encryptedMsg = rsaService.Encrypte(msg, publicKey);
        //logger.LogDebug("encryptedMsg : " + encryptedMsg);
        var decryptedMsg = rsaService.Decrypte(encryptedMsg, privateKey);
        //logger.LogDebug("decryptedMsg : " + decryptedMsg);
        if (decryptedMsg != msg) throw new Exception("something went wrong : (decryptedMsg !=  msg)");
        var signature = rsaService.Sign(msg, privateKey);
        logger.Info("signature : " + signature);
        _ = signature ?? throw new Exception("something went wrong : signature is null.");
        var verify = rsaService.Verify(msg, signature, publicKey);
        logger.Debug("verify : " + verify.ToString());
    }
    catch (Exception ex) { logger.Error("something went wrong : " + ex.Message); }
    finally
    {
        stopWatch.Stop();
        logger.Trace("RSA Encryption Demo was finished in " + stopWatch.ElapsedMilliseconds + "ms");
    }
}