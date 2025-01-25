using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace HealthQ_API.Security;

public static class HashingUtility
{
    public static (string Hash, string Salt) HashPassword(string password, string? salt = null)
    {
        var saltBytes = salt == null ? RandomNumberGenerator.GetBytes(128 / 8) : Convert.FromBase64String(salt);
        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: saltBytes,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8
        ));
        
        return (hashed, Convert.ToBase64String(saltBytes));
    }
}