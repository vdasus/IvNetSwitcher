namespace IvNetSwitcher.Core.Shared
{
    public static class Utils
    {
        public static string GetEncryptedString(string strToEncrypt, string salt)
        {
            return StringCipher.Encrypt(strToEncrypt, "C92FC156-2560-4734-9525-51E216554D25" + salt);
        }

        public static string GetDecryptedString(string strToDecrypt, string salt)
        {
            return StringCipher.Decrypt(strToDecrypt, "C92FC156-2560-4734-9525-51E216554D25" + salt);
        }
    }
}
