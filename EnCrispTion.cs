using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        // Generate and save encryption key
        byte[] encryptionKey = GenerateEncryptionKey();
        File.WriteAllBytes("encryptionKey.bin", encryptionKey);

        // Prompt for username and password
        Console.Write("Enter username: ");
        string username = Console.ReadLine();
        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        // Encrypt username and password
        string encryptedUsername = EncryptString(username, encryptionKey);
        string encryptedPassword = EncryptString(password, encryptionKey);

        // Save encrypted strings to a file
        File.WriteAllText("encryptedData.txt", encryptedUsername + "\n" + encryptedPassword);

        Console.WriteLine("Data encrypted and saved successfully.");
    }

    static byte[] GenerateEncryptionKey()
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.GenerateKey();
            return aesAlg.Key;
        }
    }

    static string EncryptString(string plainText, byte[] key)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.GenerateIV();

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                }

                return Convert.ToBase64String(aesAlg.IV.Concat(msEncrypt.ToArray()).ToArray());
            }
        }
    }
}
