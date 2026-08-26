using JapaneseLearning.User.Application.Abstractions.Security;
using Microsoft.AspNetCore.Identity;

namespace JapaneseLearning.User.Infrastructure.Security;

public sealed class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();

    public string Hash(string password)
    {
        return _hasher.HashPassword(
            null!,
            password);
    }

    public bool Verify(
        string password,
        string passwordHash)
    {
        var result = _hasher.VerifyHashedPassword(
            null!,
            passwordHash,
            password);

        return result == PasswordVerificationResult.Success
            || result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}