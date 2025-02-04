using System.Security.Cryptography;
using HealthQ_API.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace HealthQ_API.Services;

public class PasswordService
{
    public bool VerifyPasswordAsync(UserModel user, string password, CancellationToken ct)
    {
        if (user.PasswordHash != HashPassword(password, user.PasswordSalt).Hash)
            return false;
        return true;
    }
    
    public (string Hash, string Salt) HashPassword(string password, string? salt = null)
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