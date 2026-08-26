namespace JapaneseLearning.User.Application.Auth.Refresh;

public sealed record RefreshTokenResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresIn);