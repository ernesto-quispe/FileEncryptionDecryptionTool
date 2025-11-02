using System;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace FileEncryptDecryptApp
{
    // Structure with union-like behavior in C/C++
    [StructLayout(LayoutKind.Explicit)]
    struct DataUnion
    {
        [FieldOffset(0)]
        public int intValue;

        [FieldOffset(0)]
        public float floatValue;
    }

    class Encryptor
    {
        // Fixed size for AES-256 key and IV
        private const int KeySize = 32; // 256 bits
        private const int IvSize = 16;  // 128 bits

        // Fields initialized to avoid CS8618 warnings
        private byte[] _key = Array.Empty<byte>();
        private byte[] _iv = Array.Empty<byte>();

        // Empty constructor
        public Encryptor() { }

        /// <summary>
        /// Generates random AES key and IV.
        /// Method assisted by AI.
        /// </summary>
        private void GenerateKeyAndIV()
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.GenerateKey();
                aes.GenerateIV();
                _key = aes.Key;
                _iv = aes.IV;
            }
        }

        /// <summary>
        /// Encrypts a file and writes the key and IV at the beginning of the encrypted file.
        /// </summary>
        /// <param name="inputFile">Original file path</param>
        /// <param name="outputFile">Encrypted file path</param>
        public void EncryptFile(string inputFile, string outputFile)
        {
            if (!File.Exists(inputFile))
            {
                Console.WriteLine("Input file does not exist.");
                return;
            }

            GenerateKeyAndIV();

            using (FileStream inputStream = new FileStream(inputFile, FileMode.Open))
            using (FileStream outputStream = new FileStream(outputFile, FileMode.Create))
            {
                // Write key and IV for use during decryption
                outputStream.Write(_key, 0, KeySize);
                outputStream.Write(_iv, 0, IvSize);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = _key;
                    aes.IV = _iv;

                    using (CryptoStream cryptoStream = new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] buffer = new byte[1048576]; // 1MB buffer
                        int read;
                        while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            cryptoStream.Write(buffer, 0, read);
                        }
                        cryptoStream.FlushFinalBlock(); // Ensure final block is written correctly
                    }
                }
            }

            Console.WriteLine($"File encrypted successfully: {outputFile}");
        }

        /// <summary>
        /// Decrypts a file that contains the key and IV at the beginning.
        /// </summary>
        /// <param name="inputFile">Encrypted file path</param>
        /// <param name="outputFile">Decrypted file path</param>
        public void DecryptFile(string inputFile, string outputFile)
        {
            if (!File.Exists(inputFile))
            {
                Console.WriteLine("Encrypted file does not exist.");
                return;
            }

            using (FileStream inputStream = new FileStream(inputFile, FileMode.Open))
            {
                _key = new byte[KeySize];
                _iv = new byte[IvSize];

                int readKey = inputStream.Read(_key, 0, KeySize);
                int readIV = inputStream.Read(_iv, 0, IvSize);

                if (readKey != KeySize || readIV != IvSize)
                {
                    Console.WriteLine("Corrupted encrypted file or invalid format.");
                    return;
                }

                using (FileStream outputStream = new FileStream(outputFile, FileMode.Create))
                using (Aes aes = Aes.Create())
                {
                    aes.Key = _key;
                    aes.IV = _iv;

                    using (CryptoStream cryptoStream = new CryptoStream(inputStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        byte[] buffer = new byte[1048576];
                        int read;
                        while ((read = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            outputStream.Write(buffer, 0, read);
                        }
                    }
                }
            }

            Console.WriteLine($"File decrypted successfully: {outputFile}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  encrypt <input_file> <output_file>");
                Console.WriteLine("  decrypt <input_file> <output_file>");
                return;
            }

            string operation = args[0].ToLower();
            string inputFile = args[1];
            string outputFile = args[2];

            Encryptor encryptor = new Encryptor();

            DataUnion du = new DataUnion { intValue = 1067030938 };
            Console.WriteLine($"DataUnion intValue: {du.intValue}");
            Console.WriteLine($"DataUnion floatValue: {du.floatValue}");

            if (operation == "encrypt")
            {
                encryptor.EncryptFile(inputFile, outputFile);
            }
            else if (operation == "decrypt")
            {
                encryptor.DecryptFile(inputFile, outputFile);
            }
            else
            {
                Console.WriteLine("Unknown operation. Use 'encrypt' or 'decrypt'.");
            }
        }
    }
}
