namespace JapaneseLearning.User.Application.Abstractions.Security;

public interface ICurrentUser
{
    Guid? UserId { get; }

    string? Username { get; }

    string? Email { get; }

    string? Role { get; }

    bool IsAuthenticated { get; }
}