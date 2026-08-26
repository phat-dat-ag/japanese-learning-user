using System.Data;
using Dapper;
using JapaneseLearning.User.Application.Abstractions.Persistence;
using JapaneseLearning.User.Infrastructure.Database;
using RefreshTokenEntity =
    JapaneseLearning.User.Domain.Entities.RefreshToken;

namespace JapaneseLearning.User.Infrastructure.Persistence.Repositories;

public sealed class RefreshTokenRepository
    : IRefreshTokenRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public RefreshTokenRepository(
        ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task AddAsync(
        RefreshTokenEntity refreshToken,
        CancellationToken cancellationToken)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(
            new CommandDefinition(
                "dbo.usp_RefreshTokens_Create",
                new
                {
                    refreshToken.Id,
                    refreshToken.UserId,
                    refreshToken.TokenHash,
                    refreshToken.ExpiresAt,
                    refreshToken.CreatedAt
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));
    }

    public async Task<RefreshTokenEntity?> GetByTokenHashAsync(
        string tokenHash,
        CancellationToken cancellationToken)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<RefreshTokenEntity>(
            new CommandDefinition(
                "dbo.usp_RefreshTokens_GetByTokenHash",
                new
                {
                    TokenHash = tokenHash
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));
    }

    public async Task RevokeAsync(
        Guid id,
        DateTime revokedAt,
        CancellationToken cancellationToken)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(
            new CommandDefinition(
                "dbo.usp_RefreshTokens_Revoke",
                new
                {
                    Id = id,
                    RevokedAt = revokedAt
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));
    }
}