namespace JapaneseLearning.User.Application.Abstractions.Security;

public sealed record TokenResult(
    string AccessToken,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt,
    int AccessTokenExpiresIn);