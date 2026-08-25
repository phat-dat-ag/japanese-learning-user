using System.Data;
using Dapper;
using JapaneseLearning.User.Application.Abstractions.Persistence;
using JapaneseLearning.User.Infrastructure.Database;
using UserEntity = JapaneseLearning.User.Domain.Entities.User;

namespace JapaneseLearning.User.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public UserRepository(
        ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> ExistsByUsernameAsync(
        string username,
        CancellationToken cancellationToken)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        var exists = await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                "dbo.usp_Users_ExistsByUsername",
                new
                {
                    Username = username
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));

        return exists;
    }

    public async Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        var exists = await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                "dbo.usp_Users_ExistsByEmail",
                new
                {
                    Email = email
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));

        return exists;
    }

    public async Task AddAsync(
        UserEntity user,
        CancellationToken cancellationToken)
    {
        using var connection =
            _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(
            new CommandDefinition(
                "dbo.usp_Users_Create",
                new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    user.PasswordHash,
                    user.IsActive,
                    user.CreatedAt,
                    user.UpdatedAt
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));
    }
}