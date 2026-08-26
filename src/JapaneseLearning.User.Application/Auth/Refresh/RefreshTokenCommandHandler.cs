using JapaneseLearning.User.Application.Abstractions.Persistence;
using JapaneseLearning.User.Application.Abstractions.Security;
using JapaneseLearning.User.Application.Common.Exceptions;
using JapaneseLearning.User.Domain.Entities;
using MediatR;

namespace JapaneseLearning.User.Application.Auth.Refresh;

public sealed class RefreshTokenCommandHandler
    : IRequestHandler<
        RefreshTokenCommand,
        RefreshTokenResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        ITokenService tokenService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<RefreshTokenResponse> Handle(
        RefreshTokenCommand request,
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
                "Refresh token has been revoked.");
        }

        if (existingToken.ExpiresAt <= DateTime.UtcNow)
        {
            throw new UnauthorizedException(
                "REFRESH_TOKEN_EXPIRED",
                "Refresh token has expired.");
        }

        var user = await _userRepository.GetByIdAsync(
            existingToken.UserId,
            cancellationToken);

        if (user is null || !user.IsActive)
        {
            throw new UnauthorizedException(
                "INVALID_REFRESH_TOKEN",
                "Refresh token is invalid.");
        }

        var tokens = _tokenService.CreateTokens(
            user.Id,
            user.Username,
            user.Email);

        await _refreshTokenRepository.RevokeAsync(
            existingToken.Id,
            DateTime.UtcNow,
            cancellationToken);

        var newRefreshToken = new RefreshToken(
            Guid.NewGuid(),
            user.Id,
            _tokenService.HashRefreshToken(
                tokens.RefreshToken),
            tokens.RefreshTokenExpiresAt,
            DateTime.UtcNow);

        await _refreshTokenRepository.AddAsync(
            newRefreshToken,
            cancellationToken);

        return new RefreshTokenResponse(
            tokens.AccessToken,
            tokens.RefreshToken,
            tokens.AccessTokenExpiresIn);
    }
}