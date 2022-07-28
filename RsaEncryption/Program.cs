using RsaEncryption;
using RsaEncryption.Tools;
using System.Security.Cryptography;
using System.Text;

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
