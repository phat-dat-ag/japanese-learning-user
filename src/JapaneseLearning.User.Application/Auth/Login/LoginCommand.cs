using MediatR;

namespace JapaneseLearning.User.Application.Auth.Login;

public sealed record LoginCommand(
    string Email,
    string Password)
    : IRequest<LoginResponse>;