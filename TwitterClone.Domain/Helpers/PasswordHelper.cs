using System.Text;
using System.Security.Cryptography;
using System;

namespace TwitterClone.Domain.Helpers;

public class PasswordHelper
{
    public static string GenerateSalt(int saltLength)
    {
        var saltBytes = new byte[saltLength];

        using (var provider = new RNGCryptoServiceProvider())
        {
            provider.GetNonZeroBytes(saltBytes);
        }

        return Convert.ToBase64String(saltBytes);
    }

    public static string GetPasswordHashWithSalt(string password, int nIterations, int hashLength)
    {
        var salt = GenerateSalt(128);
        var saltBytes = Convert.FromBase64String(salt);

        using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, nIterations))
        {
            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(hashLength));
        }
    }
}