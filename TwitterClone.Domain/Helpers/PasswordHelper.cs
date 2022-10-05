using System.Text;
using System.Security.Cryptography;
using System;

namespace TwitterClone.Domain.Helpers;

public class PasswordHelper
{    
    public static string GetPasswordHash(string text)
    {
        string key = "some key";
        byte[] keyBytes = new ASCIIEncoding().GetBytes(key);
        byte[] textBytes = new ASCIIEncoding().GetBytes(text);

        byte[] hash = new HMACSHA256(keyBytes).ComputeHash(textBytes);
        
        String.Concat(Array.ConvertAll(hash, x => x.ToString("x2")));

        return Convert.ToBase64String(hash);
    }
}