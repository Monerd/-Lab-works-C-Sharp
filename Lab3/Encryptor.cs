using System.Security.Cryptography;
using System.IO;

namespace Lab3
{
    static class Encryptor
    {
        private static DESCryptoServiceProvider Crypto { get; set; } = new DESCryptoServiceProvider();

        public static void Encrypt(Stream sourceStream, Stream targetEncryptedStream)
        {
            using (CryptoStream ecStream = new CryptoStream(targetEncryptedStream, Crypto.CreateEncryptor(), CryptoStreamMode.Write))
            {
                sourceStream.CopyTo(ecStream);
            }
        }
    }
}
