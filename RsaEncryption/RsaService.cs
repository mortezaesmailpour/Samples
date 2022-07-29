using RsaEncryption.Tools;
using static System.Net.Mime.MediaTypeNames;

namespace RsaEncryption;
public class RsaService : IRsaService
{
    private readonly RSAEncryptionPadding encryptionPadding = RSAEncryptionPadding.Pkcs1;
    private readonly RSASignaturePadding signaturePadding = RSASignaturePadding.Pkcs1;
    private readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;
    private readonly int MaxDataSize = 245;
    private readonly int EncryptedBlockSize = 256;
    private readonly ILogger logger = new ConsoleLogger();
    public (string privateKey, string publicKey) Create()
    {
        var rsa = RSACryptoServiceProvider.Create();
        return (rsa.ToXmlString(false), rsa.ToXmlString(true));
    }
    private RSA CreateRsa(string key)
    {
        var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(key);
        return rsa;
    }
    private byte[] GetBytes(string text) => Encoding.Unicode.GetBytes(text);
    private string GetString(byte[] textBytes) => Encoding.Unicode.GetString(textBytes);
    private IList<byte[]> Split(byte[] input, int blockSize)
    {
        //Console.WriteLine($"Split({input.Length}, {blockSize})"); 
        var result = new List<byte[]>();
        var NumberOfBlock = (input.Count() / blockSize) + (input.Count() % blockSize == 0 ? 0 : 1);
        for (int i = 0; i < NumberOfBlock; i++)
        {
            result.Add(input.Skip(i * blockSize).Take(blockSize).ToArray());
        }
        return result;
    }
    private byte[] Merge(IList<byte[]> input)
    {
        //Console.Write("start merge : ");
        int totalSize = 0;
        foreach (var item in input)
        {
            totalSize += item.Length;
            //Console.Write(" + " + item.Length);
        }
        //Console.WriteLine(" = " + totalSize);
        var result = new byte[totalSize];
        int currentSize = 0;
        for (int i = 0; i < input.Count; i++)
        {
            Array.Copy(input[i], 0, result, currentSize, input[i].Length);
            currentSize += input[i].Length;
        }
        return result;
    }
    public string? Encrypte(string text, string publicKey)
    {
        var rsa = CreateRsa(publicKey);
        var textBytes = GetBytes(text);
        byte[] encryptedBytes;
        if (textBytes.Length > MaxDataSize)
        {
            logger.LogWarning($"text size ({text.Length}) in more than one block. we should split it to several Blocks");
            List<byte[]> encryptedBytesList = new List<byte[]>();
            var ListOfTextBytes = Split(textBytes, MaxDataSize);
            for (int i = 0; i < ListOfTextBytes.Count; i++)
            {
                var item = ListOfTextBytes[i];

                var encryptedItem = rsa.Encrypt(item, encryptionPadding);
                encryptedBytesList.Add(encryptedItem);
            }
            encryptedBytes = Merge(encryptedBytesList);
        }
        else
        {
            encryptedBytes = rsa.Encrypt(textBytes, encryptionPadding);
        }
        string encryptedText = Convert.ToBase64String(encryptedBytes);
        return encryptedText;
    }
    public string? Decrypte(string encrypteText, string privateKey)
    {
        var rsa = CreateRsa(privateKey);
        var encryptedBytes = Convert.FromBase64String(encrypteText);
        byte[] bytes;
        if (encryptedBytes.Length > EncryptedBlockSize)
        {
            logger.LogWarning($"encrypteText size ({encrypteText.Length}) shows it's  more than one block. we should split it to {encryptedBytes.Length / EncryptedBlockSize} Blocks");
            List<byte[]> decryptedBytesList = new List<byte[]>();
            var ListOfEncryptedBytes = Split(encryptedBytes, EncryptedBlockSize);
            Console.Write("Start Decryption ... ");
            for (int i = 0; i < ListOfEncryptedBytes.Count; i++)
            {
                var item = ListOfEncryptedBytes[i];
                var decryptedBytes = rsa.Decrypt(item, encryptionPadding);
                decryptedBytesList.Add(decryptedBytes);
                if (i%100==0) Console.Write(" .");
            }
            Console.WriteLine("Decryption was Finished.");
            bytes = Merge(decryptedBytesList);
        }
        else
        {
            bytes = rsa.Decrypt(encryptedBytes, encryptionPadding);
        }
        string Text = GetString(bytes);
        return Text;
    }
    public string? Sign(string text, string privateKey)
    {
        var rsa = CreateRsa(privateKey);
        var textBytes = GetBytes(text);
        var signatureBytes = rsa.SignData(textBytes, hashAlgorithm, signaturePadding);
        var signature = Convert.ToBase64String(signatureBytes);
        return signature;
    }
    public bool Verify(string text, string signature, string publicKey)
    {
        var rsa = CreateRsa(publicKey);
        var textBytes = GetBytes(text);
        var signatureBytes = Convert.FromBase64String(signature);
        var result = rsa.VerifyData(textBytes, signatureBytes, hashAlgorithm, signaturePadding);
        return result;
    }
}