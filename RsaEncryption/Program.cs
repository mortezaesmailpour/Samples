using RsaEncryption;
using RsaEncryption.Tools;

Console.WriteLine("RSA Encryption Demo");

RsaDemo("A");


void RsaDemo(string msg)
{
    ILogger logger = new ConsoleLogger();
    logger.LogDebug("RsaDemo started with message = " + msg);
    var stopWatch = Stopwatch.StartNew();
    try
    {
        IRsaService rsaService = new RsaService();
        (string publicKey, string privateKey) = rsaService.Create();
        logger.LogInfo("publicKey : " + publicKey);
        logger.LogInfo("privateKey : " + privateKey);

        var encryptedMsg = rsaService.Encrypte(msg, publicKey);
        logger.LogDebug("encryptedMsg : " + encryptedMsg);
        var decryptedMsg = rsaService.Decrypte(encryptedMsg, privateKey);
        logger.LogDebug("decryptedMsg : " + decryptedMsg);


        var signature = rsaService.Sign(msg, privateKey);
        logger.LogDebug("signature : " + signature);
        var verify = rsaService.Verify(msg, signature, publicKey);
        logger.LogDebug("verify : " + verify.ToString());
    }
    catch (Exception ex) { logger.LogError("something went wrong : " + ex.Message); }
    finally
    {
        stopWatch.Stop();
        logger.LogDebug("RsaDemo finished in " + stopWatch.ElapsedMilliseconds);
    }
}