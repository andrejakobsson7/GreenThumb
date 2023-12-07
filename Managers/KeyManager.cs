﻿using System.IO;
using System.Security.Cryptography;

namespace GreenThumb.Managers
{
    public static class KeyManager
    {
        public static string GetEncryptionKey()
        {
            if (File.Exists("C:\\Users\\andre\\OneDrive\\Dokument\\RestrictedSection.txt"))
            {
                return File.ReadAllText("C:\\Users\\andre\\OneDrive\\Dokument\\RestrictedSection.txt");
            }
            else
            {
                string key = GenerateEncryptionKey();
                File.WriteAllText("C:\\Users\\andre\\OneDrive\\Dokument\\RestrictedSection.txt", key);
                return key;
            }
        }

        public static string GenerateEncryptionKey()
        {
            //Create an array with place for 128 bytes and fill it with random bytes. Then convert the bytes to string and return it as the encryption-key. 
            var rng = new RNGCryptoServiceProvider();
            var filledArray = new byte[16];
            rng.GetBytes(filledArray);
            return Convert.ToBase64String(filledArray);
        }

    }
}
