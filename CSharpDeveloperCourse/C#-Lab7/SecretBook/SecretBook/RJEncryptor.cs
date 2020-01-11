namespace RJEncryption
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    public class RJEncryptor
    {
        private const string SecretKey = "123QWERTY-DefRtgb-KRONOS-QRyaQRya";

        public static string Encrypt(string text, string passwordKey)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("Error: Incorrect text for encryptor");
            }

            var rjndlSymmetryAlg = NewRijndaelManaged(passwordKey);

            var encryptor = rjndlSymmetryAlg.CreateEncryptor(rjndlSymmetryAlg.Key, rjndlSymmetryAlg.IV);

            var memoryStreamEncryptor = new MemoryStream();
            using (var cryptoStreamEncryptor = new CryptoStream(memoryStreamEncryptor, encryptor, CryptoStreamMode.Write))
            using (var streamWriterEncryptor = new StreamWriter(cryptoStreamEncryptor))
            {
                streamWriterEncryptor.Write(text);
            }

            return Convert.ToBase64String(memoryStreamEncryptor.ToArray());
        }

        public static bool IsBase64String(string base64String)
        {
            base64String = base64String.Trim();
            return (base64String.Length % 4 == 0) &&
                    Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        public static string Decrypt(string cipherText, string passwordKey)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                throw new ArgumentNullException("Error: incorrect cipherText");
            }

            if (!IsBase64String(cipherText))
            {
                throw new Exception("Error: incorrect ciperText for decryptor!");
            }

            var decText = string.Empty;

            var rjndlSymmetryAlg = NewRijndaelManaged(passwordKey);
            var decryptor = rjndlSymmetryAlg.CreateDecryptor(rjndlSymmetryAlg.Key, rjndlSymmetryAlg.IV);
            var cipherBytes = Convert.FromBase64String(cipherText);

            using (var memoryStreamDecryptor = new MemoryStream(cipherBytes))
            {
                using (var criptoStreamDecryptor = new CryptoStream(memoryStreamDecryptor, decryptor, CryptoStreamMode.Read))
                {
                    using (var streamReaderDecryptor = new StreamReader(criptoStreamDecryptor))
                    {
                        decText = streamReaderDecryptor.ReadToEnd();
                    }
                }
            }

            return decText;
        }

        private static RijndaelManaged NewRijndaelManaged(string passwordKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(passwordKey);
            var key = new Rfc2898DeriveBytes(SecretKey, keyBytes);

            var rjndlSymmetryAlg = new RijndaelManaged();

            // rjSymmetryAlg.KeySize return 256 bytes but int format requires 32 bytes => 256 / 8
            rjndlSymmetryAlg.Key = key.GetBytes(rjndlSymmetryAlg.KeySize / 8);
            rjndlSymmetryAlg.IV = key.GetBytes(rjndlSymmetryAlg.BlockSize / 8);

            return rjndlSymmetryAlg;
        }
    }
} 