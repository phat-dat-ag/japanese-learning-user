using MediatR;

namespace JapaneseLearning.User.Application.Auth.Register;

public sealed record RegisterCommand(
    string Username,
    string Email,
    string Password
) : IRequest<RegisterResponse>;