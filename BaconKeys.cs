using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new instance of RSACryptoServiceProvider
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                // Export the private key
                string privateKey = rsa.ToXmlString(true);
                File.WriteAllText("privateKey.xml", privateKey);

                // Export the public key
                string publicKey = rsa.ToXmlString(false);
                File.WriteAllText("publicKey.xml", publicKey);

                // Load the private key
                rsa.FromXmlString(privateKey);

                // Encrypt a string with the private key
                string originalText = "Hello, world!";
                byte[] dataToEncrypt = Encoding.UTF8.GetBytes(originalText);
                byte[] encryptedData = rsa.Encrypt(dataToEncrypt, false);

                // Load the public key
                rsa.FromXmlString(publicKey);

                // Decrypt the encrypted data with the public key
                byte[] decryptedData = rsa.Decrypt(encryptedData, false);
                string decryptedText = Encoding.UTF8.GetString(decryptedData);

                Console.WriteLine("Original Text: " + originalText);
                Console.WriteLine("Encrypted Text: " + Convert.ToBase64String(encryptedData));
                Console.WriteLine("Decrypted Text: " + decryptedText);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
    }
}
