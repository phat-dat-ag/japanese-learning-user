namespace JapaneseLearning.User.Application.Auth.Login;

public sealed record LoginResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresIn);