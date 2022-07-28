using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace RsaEncryption;
public class RsaService : IRsaService
{
    private readonly RSAEncryptionPadding encryptionPadding = RSAEncryptionPadding.Pkcs1;
    private readonly RSASignaturePadding signaturePadding = RSASignaturePadding.Pkcs1;
    private readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;
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
    public string? Encrypte(string text, string publicKey)
    {
        var rsa = CreateRsa(publicKey);
        var textBytes = GetBytes(text);
        var encryptedBytes = rsa.Encrypt(textBytes, encryptionPadding);
        string encryptedText = Convert.ToBase64String(encryptedBytes);
        return encryptedText;
    }

    public string? Decrypte(string encrypteText, string privateKey)
    {
        var rsa = CreateRsa(privateKey);
        var encryptedBytes = Convert.FromBase64String(encrypteText);
        var Bytes = rsa.Decrypt(encryptedBytes, encryptionPadding);
        string Text = GetString(Bytes);
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