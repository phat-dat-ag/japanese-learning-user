using MediatR;

namespace JapaneseLearning.User.Application.Auth.Logout;

public sealed record LogoutCommand(
    string RefreshToken
) : IRequest;