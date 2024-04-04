using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Security.Cryptography;

namespace Encoder
{
    class Program
    {
        static string strKey;
        static string strOffset;

        static void Main(string[] args)
        {
            MainMenu();
        }

        // Caesar Cipher Encoder with MD5 Checksum Calculation and Decryption Check
        static void Caesar_Encode(byte[] byteIn, bool isBinary = false)
        {
            Console.WriteLine("\nPlease enter an offset...");
            strOffset = Console.ReadLine().Trim();
            if (!IsValidOffset(strOffset, out int offset))
            {
                Console.WriteLine("Invalid offset. Please enter a valid number.");
                return;
            }
            Console.WriteLine("\nShaking byte sequence for payload...");

            byte[] encoded = new byte[byteIn.Length];
            for (int i = 0; i < byteIn.Length; i++)
            {
                encoded[i] = (byte)(((uint)byteIn[i] + offset) & 0xFF);
            }

            // Directly performing the reverse operation for decoding
            byte[] decoded = new byte[encoded.Length];
            for (int i = 0; i < encoded.Length; i++)
            {
                decoded[i] = (byte)(((uint)encoded[i] - offset) & 0xFF);
            }

            if (isBinary)
            {
                Console.WriteLine("\nPlease enter the path for the output binary file:");
                var outputFilePath = Console.ReadLine().Trim('"').Trim();

                // Original file MD5
                var originalMd5Checksum = ComputeMD5Checksum(byteIn);
                Console.WriteLine($"MD5 Checksum of the original file: {originalMd5Checksum}");

                WriteBinaryFile(outputFilePath, encoded); // Writing the encoded data to the binary file

                // Encoded file MD5
                var encodedMd5Checksum = ComputeMD5Checksum(encoded);
                Console.WriteLine($"MD5 Checksum of the encoded file: {encodedMd5Checksum}");

                // Decoded (recovered) file MD5
                var decodedMd5Checksum = ComputeMD5Checksum(decoded);
                Console.WriteLine($"MD5 Checksum of the decoded (recovered) file: {decodedMd5Checksum}");

                // Verification
                if (originalMd5Checksum == decodedMd5Checksum)
                {
                    Console.WriteLine("Success: The decoded file matches the original file.");
                }
                else
                {
                    Console.WriteLine("Error: The decoded file does NOT match the original file.");
                }

                Console.WriteLine("\nPress any key to return to the main menu...");
                Console.ReadKey();
                MainMenu();
            }
            else
            {
                StringBuilderService(encoded, "encoded Caesar", strOffset);
            }
        }

        // Caesar Cipher Decoder
        static void CaesarDecode(byte[] byteIn, bool isBinary = false)
        {
            Console.WriteLine("\nPlease enter an offset...");
            strOffset = Console.ReadLine().Trim();
            if (!IsValidOffset(strOffset, out int offset))
            {
                Console.WriteLine("Invalid offset. Please enter a valid number.");
                return;
            }

            byte[] decoded = new byte[byteIn.Length];
            for (int i = 0; i < byteIn.Length; i++)
            {
                decoded[i] = (byte)(((uint)byteIn[i] - offset) & 0xFF);
            }

            if (isBinary)
            {
                Console.WriteLine("\nPlease enter the path for the output binary file:");
                var outputFilePath = Console.ReadLine().Trim('"').Trim();

                WriteBinaryFile(outputFilePath, decoded); // Directly write the encoded data to the binary file

                // Compute and display the MD5 checksum
                var md5Checksum = ComputeMD5Checksum(decoded);
                Console.WriteLine($"MD5 Checksum of the encoded file: {md5Checksum}");

                Console.WriteLine("\nPress any key to return to the main menu...");
                Console.ReadKey();
                MainMenu();
            }
            else
            {
                StringBuilderService(decoded, "decoded Caesar", strOffset);
            }
        }

        // Xor Encoder / Decoder
        static void XorEncode(byte[] byteIn, string strOption, bool isBinary = false)
        {
            Console.WriteLine("\nPlease enter an encryption key...");
            strKey = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(strKey))
            {
                Console.WriteLine("Encryption key cannot be empty.");
                return;
            }

            byte[] processed = new byte[byteIn.Length];
            for (int i = 0; i < byteIn.Length; i++)
            {
                processed[i] = (byte)(byteIn[i] ^ strKey[i % strKey.Length]);
            }

            if (isBinary)
            {
                // Original file MD5 for comparison after decode
                var originalMd5Checksum = ComputeMD5Checksum(byteIn);
                Console.WriteLine($"MD5 Checksum of the original file: {originalMd5Checksum}");

                if (strOption == "1")
                    Console.WriteLine("Encrypting byte sequence for payload...");
                else if (strOption == "3")
                    Console.WriteLine("Decrypting byte sequence for payload...");

                // Directly prompt for output file path and write the processed data
                Console.WriteLine("\nPlease enter the path for the output binary file:");
                var outputFilePath = Console.ReadLine().Trim('"').Trim();

                WriteBinaryFile(outputFilePath, processed);

                // Compute and display the MD5 checksum
                var processedMd5Checksum = ComputeMD5Checksum(processed);
                Console.WriteLine($"MD5 Checksum of the {(strOption == "1" ? "encoded" : "decoded")} file: {processedMd5Checksum}");

                // If encoding, decrypt to verify the process is reversible
                if (strOption == "1")
                {
                    byte[] recovered = new byte[processed.Length];
                    for (int i = 0; i < processed.Length; i++)
                    {
                        recovered[i] = (byte)(processed[i] ^ strKey[i % strKey.Length]); // XOR again for decryption
                    }

                    var recoveredMd5Checksum = ComputeMD5Checksum(recovered);
                    Console.WriteLine($"MD5 Checksum of the recovered file: {recoveredMd5Checksum}");

                    if (originalMd5Checksum == recoveredMd5Checksum)
                    {
                        Console.WriteLine("Success: The recovered file matches the original file.");
                    }
                    else
                    {
                        Console.WriteLine("Error: The recovered file does NOT match the original file.");
                    }
                }

                Console.WriteLine("\nPress any key to return to the main menu...");
                Console.ReadKey();
                MainMenu();
            }
            else
            {
                // Use existing StringBuilderService for textual output (non-binary)
                if (strOption == "1")
                    StringBuilderService(processed, "encoded Xor", strKey);
                else if (strOption == "3")
                    StringBuilderService(processed, "decoded Xor", strKey);
            }
        }

        // String Builder Service formats output to a C# byte array when required
        static void StringBuilderService(byte[] byteIn, string strIn, string strKey)
        {
            StringBuilder hex = new StringBuilder(byteIn.Length * 2);
            foreach (byte b in byteIn)
            {
                hex.AppendFormat("0x{0:x2},", b);
            }

            string output = hex.ToString().TrimEnd(',');
            string strNewPayload = ($"byte[] buf = new byte[{output.Split(',').Length}] {{ {output} }};");

            Console.WriteLine($"The {strIn} Payload is:");
            Console.WriteLine(strNewPayload);
            Console.WriteLine($"\nThe offset is: {strKey}");
            MainMenu();
        }

        // Validity check for Caesar cipher offset
        static bool IsValidOffset(string input, out int offset)
        {
            return int.TryParse(input, out offset) && offset >= 0;
        }

        // Main Menu
        static void MainMenu()
        {
            Console.WriteLine(@"
 _____                     _           
| ____|_ __   ___ ___   __| | ___ _ __ 
|  _| | '_ \ / __/ _ \ / _` |/ _ \ '__|
| |___| | | | (_| (_) | (_| |  __/ |   
|_____|_| |_|\___\___/ \__,_|\___|_|   ");
            Console.WriteLine("\n Written by: g00s3");

            var arrMenu = new[] { "\n\n\nPlease enter an option:", "[0] - C# Byte Array",
                "[1] - Bin File / Shellcode", "[x] - Exit\n" };
            Console.WriteLine(String.Join(Environment.NewLine, arrMenu));
            MainSelector(Console.ReadLine().Trim());
        }
        // Main Menu Selector
        static void MainSelector(String strIn)
        {
            try
            {
                if (strIn == "x")
                    Environment.Exit(0);

                if (strIn == "0")
                    byteArrayMenu();
                else if (strIn == "1")
                    shellcodeMenu();
            }
            catch (Exception strException)
            {
                Console.WriteLine(strException);
            }
        }
        // Byte Arrays
        static void byteArrayMenu()
        {
            var arrMenu = new[] { "\nEncoding C# byte Arrays\nPlease enter an option:", "[0] - Encode Caesar Cipher",
                "[1] - Xor Encode", "[2] - Decode Caesar Cipher", "[3] - Xor Decode", "[x] - Exit\n" };
            Console.WriteLine(String.Join(Environment.NewLine, arrMenu));
            byteArraySelector(Console.ReadLine().Trim());
        }

        static void byteArraySelector(String strIn)
        {
            try
            {
                if (strIn == "x")
                    Environment.Exit(0);

                Console.WriteLine("\nPlease paste the payload to encode:");
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

        // Shellcode / Binary
        static void shellcodeMenu()
        {
            var arrMenu = new[] { "\n\n\nBinary Shellcode - Please enter an option:", "[0] - Encode Caesar Cipher",
                "[1] - Xor Encode", "[2] - Decode Caesar Cipher", "[3] - Xor Decode", "[x] - Exit\n" };
            Console.WriteLine(String.Join(Environment.NewLine, arrMenu));
            shellcodeSelector(Console.ReadLine().Trim());
        }

        static void shellcodeSelector(String strIn)
        {
            try
            {
                if (strIn == "x")
                    Environment.Exit(0);

                Console.WriteLine("\nPlease enter the path to the binary file:");
                var inputFilePath = Console.ReadLine();

                byte[] buf = ReadBinaryFile(inputFilePath);
                if (buf == null || buf.Length == 0)
                {
                    Console.WriteLine("Error reading file or file is empty.");
                    return;
                }

                // Perform the encoding or decoding operation
                if (strIn == "0")
                    Caesar_Encode(buf, true);
                else if (strIn == "1")
                    XorEncode(buf, "1", true);
                else if (strIn == "2")
                    CaesarDecode(buf, true);
                else if (strIn == "3")
                    XorEncode(buf, "3", true);

                // Prompt for output file path
                Console.WriteLine("\nPlease enter the path for the output file:");
                var outputFilePath = Console.ReadLine();

                // Use the modified buffer after encoding/decoding
                WriteBinaryFile(outputFilePath, buf);
            }
            catch (Exception strException)
            {
                Console.WriteLine(strException.Message);
            }
        }

        // Allows the user to copy and paste a multiline payload to the application
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

        // Read Binary File 
        static byte[] ReadBinaryFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                Console.WriteLine("File path is empty or null.");
                return null;
            }

            filePath = filePath.Trim('"');

            if (!IsValidFilePath(filePath))
            {
                Console.WriteLine("File path is invalid or contains illegal characters.");
                return null;
            }

            try
            {
                // Read all bytes from the specified file into a byte array.
                byte[] fileContent = File.ReadAllBytes(filePath);
                // Compute and display the MD5 checksum
                var md5Checksum = ComputeMD5Checksum(fileContent);
                Console.WriteLine($"MD5 Checksum of the original file: {md5Checksum}");
                return fileContent;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
                return null; // or handle the error as appropriate for your application
            }
        }

        // Write Binary File
        static void WriteBinaryFile(string filePath, byte[] data)
        {
            filePath = filePath.Trim('"').Trim();

            try
            {
                // Write all bytes from the specified file into a byte array.
                File.WriteAllBytes(filePath, data);
                Console.WriteLine($"Data successfully written to file: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
            }
        }

        static void PromptForOutputAndWrite(byte[] data)
        {
            Console.WriteLine("\nPlease enter the path for the output binary file:");
            var outputFilePath = Console.ReadLine();

            outputFilePath = outputFilePath.Trim('"');

            WriteBinaryFile(outputFilePath, data);

            // Go back to main menu
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey(); // Wait for the user to acknowledge before moving on
            MainMenu();
        }

        // Check for valid file paths
        static bool IsValidFilePath(string filePath)
        {
            try
            {
                var fileName = Path.GetFileName(filePath); // This will throw if the path is invalid
                var fullPath = Path.GetFullPath(filePath); // This checks if the path format is correct
                return true;
            }
            catch
            {
                return false;
            }
        }

        // MD5 Checks
        static string ComputeMD5Checksum(byte[] data)
        {
            using (var md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(data);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // Convert byte to Hexadecimal string
                }
                return sb.ToString();
            }
        }
    }
}