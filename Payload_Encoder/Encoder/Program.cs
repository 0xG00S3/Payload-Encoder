using System;
using System.IO;
using System.Text;
using System.Linq;

namespace Encoder
{
    class Program
    {
        static string strKey;
        static string strOffset;

        static void Main(string[] args)
        {
            Menu();
        }

        static void Caesar_Encode(byte[] byteIn)
        {
            Console.WriteLine("\nPlease enter an offset...");
            strOffset = Console.ReadLine().Trim();
            Console.WriteLine("\nShaking byte sequence for payload...");

            byte[] encoded = new byte[byteIn.Length];
            for (int i = 0; i < byteIn.Length; i++)
            {
                encoded[i] = (byte)(((uint)byteIn[i] + Convert.ToInt32(strOffset)) & 0xFF);
            }
            StringBuilderService(encoded, "encoded Caesar", strOffset);
        }

        static void XorEncode(byte[] byteIn, string strOption)
        {
            Console.WriteLine("\nPlease enter an encryption key...");
            strKey = Console.ReadLine().Trim();

            if (strOption == "1")
                Console.WriteLine("Encrypting byte sequence for payload...");
            else if (strOption == "3")
                Console.WriteLine("Decrypting byte sequence for payload...");

            for (int i = 0; i < byteIn.Length; i++)
            {
                byteIn[i] = (byte)(byteIn[i] ^ strKey[i % strKey.Length]);
            }
            if (strOption == "1")
                StringBuilderService(byteIn, "encoded Xor", strKey);
            else if (strOption == "3")
                StringBuilderService(byteIn, "decoded Xor", strKey);
        }

        static void CaesarDecode(byte[] byteIn)
        {
            Console.WriteLine("\nPlease enter an offset...");
            strOffset = Console.ReadLine().Trim();
            Console.WriteLine("Shaking byte sequence...");

            for (int i = 0; i < byteIn.Length; i++)
            {
                byteIn[i] = (byte)(((uint)byteIn[i] - Convert.ToInt32(strOffset) & 0xFF));
            }
            StringBuilderService(byteIn, "decoded Caesar", strOffset);
        }

        static void StringBuilderService(byte[] byteIn, string strIn, string strKey)
        {
            StringBuilder hex = new StringBuilder(byteIn.Length * 2);
            foreach (byte b in byteIn)
            {
                hex.AppendFormat("0x{0:x2},", b);
            }

            string output = hex.ToString();
            output = output.Remove(output.Length - 1, 1);
            string strNewPayload = ("byte[] buf = new byte[" + output.Split(',').Length + "] { " + output + " };");

            Console.WriteLine($"The {strIn} Payload is:");
            Console.WriteLine(strNewPayload);
            Console.WriteLine($"\nThe offset is: {strKey}");
            Menu();
        }

        static void Menu()
        {
            var arrMenu = new[] { "\n\nWelcome to the encoder.", "Please enter an option:", "[0] - Encode Caesar Cipher",
                "[1] - Xor Encode", "[2] - Decode Caesar Cipher", "[3] - Xor Decode", "[x] - Exit\n" };
            Console.WriteLine(String.Join(Environment.NewLine, arrMenu));
            Selector(Console.ReadLine().Trim());
        }

        static void Selector(String strIn)
        {
            try
            {
                if (strIn == "x")
                    Environment.Exit(0);
                
                var byteString = BetterReadLine().Replace("\r\n", "");

                byte[] buf = byteString.Split(',').Select(x => Convert.ToByte(x, 16)).ToArray();

                if (strIn == "0")
                    Caesar_Encode(buf);
                else if (strIn == "1")
                    XorEncode(buf, strIn);
                else if (strIn == "2")
                    CaesarDecode(buf);
                else if (strIn == "3")
                    XorEncode(buf, strIn);
            }
            catch (Exception strException)
            {
                Console.WriteLine(strException);
            }
        }
        /*
        static string BetterReadLine()
        {
            const int BUF_MAX = 10000;
            Stream inputStream = Console.OpenStandardInput();
            byte[] bytes = new byte[BUF_MAX];
            int outputLength = inputStream.Read(bytes, 0, BUF_MAX);
            char[] chars = Encoding.UTF8.GetChars(bytes, 0, outputLength);
            return new string(chars).Trim();
        }
        */

        static string BetterReadLine()
        {
            StringBuilder inputBuilder = new StringBuilder();
            Console.WriteLine("\nEnter payload, line by line. Type 'END' (in a new line) when done:");

            string inputLine;
            while (true)
            {
                inputLine = Console.ReadLine();
                if (inputLine == "END") // Check for terminator
                    break;
                inputBuilder.AppendLine(inputLine);
            }

            return inputBuilder.ToString().TrimEnd(); // TrimEnd to remove the last new line added by AppendLine
        }
    }
}