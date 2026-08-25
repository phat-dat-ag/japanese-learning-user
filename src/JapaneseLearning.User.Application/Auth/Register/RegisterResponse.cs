namespace JapaneseLearning.User.Application.Auth.Register;

public sealed record RegisterResponse(
    Guid Id,
    string Username,
    string Email,
    DateTime CreatedAt
);