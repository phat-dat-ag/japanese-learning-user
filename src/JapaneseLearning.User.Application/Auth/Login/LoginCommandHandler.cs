using JapaneseLearning.User.Application.Abstractions.Persistence;
using JapaneseLearning.User.Application.Abstractions.Security;
using JapaneseLearning.User.Application.Common.Exceptions;
using JapaneseLearning.User.Domain.Entities;
using MediatR;

namespace JapaneseLearning.User.Application.Auth.Login;

public sealed class LoginCommandHandler
    : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IRefreshTokenRepository refreshTokenRepository,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var email = request.Email
            .Trim()
            .ToLowerInvariant();

        var user = await _userRepository.GetByEmailAsync(
            email,
            cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedException(
                "INVALID_CREDENTIALS",
                "Invalid email or password.");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedException(
                "USER_INACTIVE",
                "User account is inactive.");
        }

        var passwordValid = _passwordHasher.Verify(
            request.Password,
            user.PasswordHash);

        if (!passwordValid)
        {
            throw new UnauthorizedException(
                "INVALID_CREDENTIALS",
                "Invalid email or password.");
        }

        var tokens = _tokenService.CreateTokens(
            user.Id,
            user.Username,
            user.Email,
            user.Role);

        var refreshToken = new RefreshToken(
            Guid.NewGuid(),
            user.Id,
            _tokenService.HashRefreshToken(
                tokens.RefreshToken),
            tokens.RefreshTokenExpiresAt,
            DateTime.UtcNow);

        await _refreshTokenRepository.AddAsync(
            refreshToken,
            cancellationToken);

        return new LoginResponse(
            tokens.AccessToken,
            tokens.RefreshToken,
            tokens.AccessTokenExpiresIn);
    }
}