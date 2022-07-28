using RsaEncryption;
using RsaEncryption.Tools;

Console.WriteLine("RSA Encryption Demo");
ILogger logger = new ConsoleLogger();

var msg = "Hello World";


IRsaService rsaService = new RsaService();
(string publicKey, string privateKey) = rsaService.Create();
logger.LogInfo("publicKey : " + publicKey);
logger.LogInfo("privateKey : " + privateKey);

logger.LogDebug("Msg : " + msg);
var encryptedMsg = rsaService.Encrypte(msg, publicKey);
logger.LogDebug("encryptedMsg : " + encryptedMsg);
var decryptedMsg = rsaService.Decrypte(encryptedMsg, privateKey);
logger.LogDebug("decryptedMsg : " + decryptedMsg);


logger.LogDebug("Msg : " + msg);
var signature = rsaService.Sign(msg, privateKey);
logger.LogDebug("signature : " + signature);
var verify = rsaService.Verify(msg, signature, publicKey);
logger.LogDebug("verify : " + verify.ToString());