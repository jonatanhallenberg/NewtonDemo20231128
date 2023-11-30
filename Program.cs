using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualBasic;
using Aes = System.Security.Cryptography.Aes;

namespace NewtonDemo20231128;

class Program
{
    static void Main(string[] args)
    {
        //Console.WriteLine("Hello, World!");
        // string encryptedText = EncryptWithKeyUsingBytes("Hello world!", "hfjd746fh3b");
        // Console.WriteLine(encryptedText);

        // string decryptedText = DecryptWithKeyUsingBytes("IAMGCFgUQQkaXwZJ", "hfjd746fh3b");
        // Console.WriteLine(decryptedText);

        string encryptedText = EncryptWithLib("Hello world!");
        Console.WriteLine(encryptedText);
    }

    private static readonly byte[] Key = Encoding.UTF8.GetBytes("djencury27shrjfkdleosiqjxhsk48Ds");
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("jeudhcnsmakw34gf");
    
    static string EncryptWithLib(string text)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            var encryptor = aes.CreateEncryptor();

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (var streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(text);
                    }
                }
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }

    static string EncryptWithKey(string text, string key)
    {
        //string text = "Hello world!"
        //string key = "hfjd746fh3b"

        string encryptedText = "";
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            int keyPos = i % key.Length;
            char k = key[keyPos];
            encryptedText += (char)(c ^ k);
        }
        return encryptedText;
    }

    static string EncryptWithKeyUsingBytes(string text, string key)
    {
        byte[] textBytes = Encoding.UTF8.GetBytes(text);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] encryptedBytes = new byte[textBytes.Length];

        for (int i = 0; i < textBytes.Length; i++)
        {
            encryptedBytes[i] = (byte)(textBytes[i] ^ keyBytes[i % keyBytes.Length]);
        }


        return Convert.ToBase64String(encryptedBytes);
    }

    static string DecryptWithKeyUsingBytes(string text, string key) {

        byte[] textBytes = Convert.FromBase64String(text);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);

        byte[] encryptedBytes = new byte[textBytes.Length];

        for (int i = 0; i < textBytes.Length; i++)
        {
            encryptedBytes[i] = (byte)(textBytes[i] ^ keyBytes[i % keyBytes.Length]);
        }

        return Encoding.UTF8.GetString(encryptedBytes);
    }

    static string EncryptWithShiffer(string text, int shift)
    {
        // [ 'H', 'e', 'l', 'l' ]
        char[] buffer = text.ToCharArray();

        for (int i = 0; i < buffer.Length; i++)
        {
            char letter = buffer[i];
            letter = (char)(letter + shift);

            if (letter > 'z')
            {
                letter = (char)(letter - 26);
            }
            else if (letter < 'a')
            {
                letter = (char)(letter + 26);
            }

            buffer[i] = letter;
        }

        return new string(buffer);
    }
}
