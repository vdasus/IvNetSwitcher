namespace IvNetSwitcher.Core.Abstractions
{
    public interface IUtilsService
    {
        void SetSalt(string salt);
        string GetEncryptedString(string strToEncrypt);
        string GetDecryptedString(string strToDecrypt);
    }
}