using RsaEncryption;
using RsaEncryption.Tools;
ILogger logger = new ConsoleLogger();

Console.WriteLine("RSA Encryption Demo");

RsaDemo(GetRandomString( 1001007));


string GetRandomString(int length)
{
    Random rand = new Random();
    int randValue;
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

void RsaDemo(string msg)
{
    //logger.LogDebug("RsaDemo started with message = " + msg);
    var stopWatch = Stopwatch.StartNew();
    try
    {
        IRsaService rsaService = new RsaService();
        (string publicKey, string privateKey) = rsaService.Create();
        logger.LogInfo("publicKey : " + publicKey);
        logger.LogInfo("privateKey : " + privateKey);
        //logger.LogDebug("msg : " + msg);
        logger.LogInfo("msg size : " + msg.Length);
        var encryptedMsg = rsaService.Encrypte(msg, publicKey);
        //logger.LogDebug("encryptedMsg : " + encryptedMsg);
        var decryptedMsg = rsaService.Decrypte(encryptedMsg, privateKey);
        //logger.LogDebug("decryptedMsg : " + decryptedMsg);
        if (decryptedMsg!= msg)
        {
            logger.LogError("something went wrong : (decryptedMsg !=  msg)" );
        }

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