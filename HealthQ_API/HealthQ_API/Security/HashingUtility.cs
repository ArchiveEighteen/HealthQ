using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace HealthQ_API.Security;

public static class HashingUtility
{
    public static (string Hash, string Salt) HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(128 / 8);
        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8
        ));
        
        return (hashed, Convert.ToBase64String(salt));
    }
}