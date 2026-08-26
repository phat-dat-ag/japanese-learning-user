using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JapaneseLearning.User.Application.Abstractions.Security;
using JapaneseLearning.User.Domain.Users;
using JapaneseLearning.User.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JapaneseLearning.User.Infrastructure.Security;

public sealed class TokenService : ITokenService
{
    private readonly JwtOptions _options;

    public TokenService(
        IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public TokenResult CreateTokens(
        Guid userId,
        string username,
        string email,
        UserRole role)
    {
        var accessToken = GenerateAccessToken(
            userId,
            username,
            email,
            role);

        var refreshToken =
            GenerateRefreshToken();

        var refreshTokenExpiresAt =
            DateTime.UtcNow.AddDays(
                _options.RefreshTokenExpirationDays);

        return new TokenResult(
            accessToken,
            refreshToken,
            refreshTokenExpiresAt,
            _options.AccessTokenExpirationMinutes * 60);
    }

    public string HashRefreshToken(
        string refreshToken)
    {
        var bytes = SHA256.HashData(
            Encoding.UTF8.GetBytes(refreshToken));

        return Convert.ToHexString(bytes);
    }

    private string GenerateAccessToken(
        Guid userId,
        string username,
        string email,
        UserRole role)
    {
        var claims = new[]
        {
            new Claim(
                JwtRegisteredClaimNames.Sub,
                userId.ToString()),

            new Claim(
                JwtRegisteredClaimNames.UniqueName,
                username),

            new Claim(
                JwtRegisteredClaimNames.Email,
                email),

            new Claim(
                ClaimTypes.Role,
                role.ToString()),

            new Claim(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_options.Secret));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                _options.AccessTokenExpirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var bytes =
            RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(bytes);
    }
}