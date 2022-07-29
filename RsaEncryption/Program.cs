using RsaEncryption;

ILogger logger = new ConsoleLogger();

logger.Test();

RSAEncryptionDemo(GetRandomString(1001007));


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