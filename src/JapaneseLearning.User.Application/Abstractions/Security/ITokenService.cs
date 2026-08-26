using JapaneseLearning.User.Domain.Users;

namespace JapaneseLearning.User.Application.Abstractions.Security;

public interface ITokenService
{
    TokenResult CreateTokens(
        Guid userId,
        string username,
        string email,
        UserRole role);

    string HashRefreshToken(
        string refreshToken);
}