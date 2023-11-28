using System.Text;

namespace NewtonDemo20231128;

class Program
{
    static void Main(string[] args)
    {
        //Console.WriteLine("Hello, World!");
        string encryptedText = EncryptWithKeyUsingBytes("Hello world!", "hfjd746fh3b");
        Console.WriteLine(encryptedText);
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
