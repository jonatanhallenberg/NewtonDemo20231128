namespace NewtonDemo20231128;

class Program
{
    static void Main(string[] args)
    {
        //Console.WriteLine("Hello, World!");
        string encryptedText = Encrypt(",", 14);
        Console.WriteLine(encryptedText);
    }

    static string Encrypt(string text, int shift)
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
