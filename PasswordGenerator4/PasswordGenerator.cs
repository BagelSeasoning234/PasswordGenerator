using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator4
{
    enum EPasswordType
    {
        Lowercase,
        Uppercase,
        Numbers,
        AvoidAmbiguous,
        AvoidSymbols,
        AvoidAmbiguousAndSymbols,
        All
    }

    class PasswordGenerator
    {

        /// <summary>
        /// Generates a password with the given length
        /// </summary>
        /// <param name="length">The number of characters</param>
        /// <param name="passwordType">The type of password to generate</param>
        /// <returns></returns>
        public string GeneratePassword (int length, EPasswordType passwordType)
        {

            // Makes sure the given arguments are valid.

            if (length < 0 || length == 0)
                throw new ArgumentException("Password length must be valid.", "length");

            if (length > int.MaxValue / 8)
                throw new ArgumentException("Password length exceeds maximum value.", "length");

            var characterArray = GetCharacterSet(passwordType).Distinct().ToArray();

            if (characterArray.Length == 0)
                throw new ArgumentException("Character set must not be empty.", "passwordType");

            // Creates a byte array of the desired length, and fills it with random values.

            byte[] bytes = new byte[length * 8];
            new RNGCryptoServiceProvider().GetBytes(bytes);

            // Fills a char array with random values from the target character set using the bytes generated earlier.

            char[] result = new char[length];

            for (int i = 0; i < length; i++)
            {
                ulong value = BitConverter.ToUInt64(bytes, i * 8);
                result[i] = characterArray[value % (uint)characterArray.Length];
            }

            return new string(result);
        }

        /// <summary>
        /// Returns the character set to use, depending on the type of password chosen.
        /// </summary>
        /// <param name="passwordType"></param>
        /// <returns></returns>
        private string GetCharacterSet(EPasswordType passwordType)
        {

            const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string Numbers = "1234567890";
            const string NoAmbiguous = "abcdefghjkmnpqrstuvwxyzABCDEFGHJKMNPQRSTUVWXYZ23456789!@#$%^&*-_=+><";
            const string NoSymbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            const string NoAmbiguousOrSymbols = "abcdefghjkmnpqrstuvwxyzABCDEFGHJKMNPQRSTUVWXYZ23456789";
            const string AllCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-_=+][}{/.,><;:'`~";

            switch (passwordType)
            {
                case EPasswordType.Lowercase:
                    return Lowercase;

                case EPasswordType.Uppercase:
                    return Uppercase;

                case EPasswordType.Numbers:
                    return Numbers;

                case EPasswordType.AvoidAmbiguous:
                    return NoAmbiguous;

                case EPasswordType.AvoidSymbols:
                    return NoSymbols;

                case EPasswordType.AvoidAmbiguousAndSymbols:
                    return NoAmbiguousOrSymbols;

                case EPasswordType.All:
                    return AllCharacters;

                default:
                    return string.Empty;

            }
        }
    }
}
