using System.Text;
using System.Security.Cryptography;
using System;

namespace TwitterClone.Domain.Helpers;

public class PasswordHelper
{
    public static string GetSha256Hash(string inputData)
    {
        using (var sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(inputData));

            var builder = new StringBuilder();
            for (int i=0; i<bytes.Length; ++i)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
    public static string HashString(string text, string salt = "")
    {
        if (String.IsNullOrEmpty(text))
        {
            return String.Empty;
        }

        using (var sha = new SHA256Managed())
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(text+salt);
            byte[] hashBytes = sha.ComputeHash(textBytes);

            var hash = BitConverter
                .ToString(hashBytes)
                .Replace("-", String.Empty);
            
            return hash;
        }        
    }
}