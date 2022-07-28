namespace RsaEncryption;
public interface IRsaService
{
    (string privateKey, string publicKey) Create();
    string? Encrypte(string text, string publicKey);
    string? Decrypte(string encrypteText, string privateKey);
    string? Sign(string text, string privateKey);
    bool Verify(string text, string signature, string publicKey);
}