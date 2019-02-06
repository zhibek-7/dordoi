using System.Security.Cryptography;
using System.Text;

using System;
//using System.Security.Cryptography;
//using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Utilities.Cryptography
{
    public static class CryptographyProvider
    {
        static public string GetMD5Hash(string input)
        {
            input = input + "lsbq85hiz";

            MD5 md5Hash = new MD5CryptoServiceProvider();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        //static public string GetPbkdf2HMACSHA256Hash(string password)
        //{
        //    // generate a 128-bit salt using a secure PRNG
        //    byte[] salt = new byte[128 / 8];
        //    using (var rng = RandomNumberGenerator.Create())
        //    {
        //        rng.GetBytes(salt);
        //    }
        //    // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
        //    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        //        password: password,
        //        salt: salt,
        //        prf: KeyDerivationPrf.HMACSHA1,
        //        iterationCount: 10000,
        //        numBytesRequested: 256 / 8));

        //    return hashed;
        //}
    }
}
