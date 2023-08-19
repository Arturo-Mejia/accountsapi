using System.Security.Cryptography;
using System.Text;

namespace apitest.Model
{
    public class SecurityPass
    {
        private static readonly string Key = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("keys")["key"];
        public static string Encrypt(string password)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Key);
                aesAlg.GenerateIV(); // Genera un vector de inicialización aleatorio

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(password);
                        }
                    }
                    byte[] encryptedBytes = msEncrypt.ToArray();
                    byte[] combinedBytes = new byte[aesAlg.IV.Length + encryptedBytes.Length];
                    Buffer.BlockCopy(aesAlg.IV, 0, combinedBytes, 0, aesAlg.IV.Length);
                    Buffer.BlockCopy(encryptedBytes, 0, combinedBytes, aesAlg.IV.Length, encryptedBytes.Length);
                    return Convert.ToBase64String(combinedBytes);
                }
            }
        }

        public static string Decrypt(string encryptedPassword)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Key);

                byte[] combinedBytes = Convert.FromBase64String(encryptedPassword);
                byte[] iv = new byte[aesAlg.IV.Length];
                byte[] encryptedBytes = new byte[combinedBytes.Length - aesAlg.IV.Length];
                Buffer.BlockCopy(combinedBytes, 0, iv, 0, aesAlg.IV.Length);
                Buffer.BlockCopy(combinedBytes, aesAlg.IV.Length, encryptedBytes, 0, encryptedBytes.Length);

                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
