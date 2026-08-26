using JapaneseLearning.User.Application.Abstractions.Persistence;
using JapaneseLearning.User.Application.Abstractions.Security;
using JapaneseLearning.User.Application.Common.Exceptions;
using MediatR;

namespace JapaneseLearning.User.Application.Auth.Logout;

public sealed class LogoutCommandHandler
    : IRequestHandler<LogoutCommand>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenService _tokenService;

    public LogoutCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        ITokenService tokenService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService = tokenService;
    }

    public async Task Handle(
        LogoutCommand request,
        CancellationToken cancellationToken)
    {
        var tokenHash =
            _tokenService.HashRefreshToken(
                request.RefreshToken);

        var existingToken =
            await _refreshTokenRepository.GetByTokenHashAsync(
                tokenHash,
                cancellationToken);

        if (existingToken is null)
        {
            throw new UnauthorizedException(
                "INVALID_REFRESH_TOKEN",
                "Refresh token is invalid.");
        }

        if (existingToken.RevokedAt.HasValue)
        {
            throw new UnauthorizedException(
                "REFRESH_TOKEN_REVOKED",
                "Refresh token has already been revoked.");
        }

        if (existingToken.ExpiresAt <= DateTime.UtcNow)
        {
            throw new UnauthorizedException(
                "REFRESH_TOKEN_EXPIRED",
                "Refresh token has expired.");
        }

        await _refreshTokenRepository.RevokeAsync(
            existingToken.Id,
            DateTime.UtcNow,
            cancellationToken);
    }
}