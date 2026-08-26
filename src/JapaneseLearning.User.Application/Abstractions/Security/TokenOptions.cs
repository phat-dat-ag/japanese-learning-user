namespace JapaneseLearning.User.Application.Abstractions.Security;

public sealed class TokenOptions
{
    public int AccessTokenExpirationMinutes { get; init; }

    public int RefreshTokenExpirationDays { get; init; }
}