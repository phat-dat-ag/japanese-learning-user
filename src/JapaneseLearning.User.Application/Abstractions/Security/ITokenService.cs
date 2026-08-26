namespace JapaneseLearning.User.Application.Abstractions.Security;

public interface ITokenService
{
    TokenResult CreateTokens(
        Guid userId,
        string username,
        string email);

    string HashRefreshToken(
        string refreshToken);
}