using MediatR;

namespace JapaneseLearning.User.Application.Auth.Refresh;

public sealed record RefreshTokenCommand(
    string RefreshToken)
    : IRequest<RefreshTokenResponse>;