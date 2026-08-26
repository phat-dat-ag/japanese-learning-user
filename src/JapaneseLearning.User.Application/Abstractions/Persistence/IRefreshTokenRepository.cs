using RefreshTokenEntity =
    JapaneseLearning.User.Domain.Entities.RefreshToken;

namespace JapaneseLearning.User.Application.Abstractions.Persistence;

public interface IRefreshTokenRepository
{
    Task AddAsync(
        RefreshTokenEntity refreshToken,
        CancellationToken cancellationToken);

    Task<RefreshTokenEntity?> GetByTokenHashAsync(
        string tokenHash,
        CancellationToken cancellationToken);

    Task RevokeAsync(
        Guid id,
        DateTime revokedAt,
        CancellationToken cancellationToken);
}