using System.Text;
using System.Security.Cryptography;

namespace TwitterClone.Domain.Helpers;

public static class SHA256HashProvider
{
    public static string GetSHA256Hash(string inputText)
    {
        using (SHA256 sha256Hash = SHA256.Create())  
            {  
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(inputText));  
  
                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();  
                for (int i = 0; i < bytes.Length; i++)  
                {  
                    builder.Append(bytes[i].ToString("x2"));  
                }  
                return builder.ToString();  
            }  
    }
}