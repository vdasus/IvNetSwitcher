using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Shared;

namespace IvNetSwitcher.Core.DomainServices
{
    public class UtilsService: IUtilsService
    {
        private const string APP_GUID = "730DE481-2077-4D59-AF65-543669121202";

        private string _salt = APP_GUID;

        public void SetSalt(string salt)
        {
            _salt = salt;
        }

        public string GetEncryptedString(string strToEncrypt)
        {
            return StringCipher.Encrypt(strToEncrypt, APP_GUID + _salt);
        }

        public string GetDecryptedString(string strToDecrypt)
        {
            return StringCipher.Decrypt(strToDecrypt, APP_GUID + _salt);
        }
    }
}
