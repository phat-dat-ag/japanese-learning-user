using UserEntity = JapaneseLearning.User.Domain.Entities.User;

namespace JapaneseLearning.User.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<bool> ExistsByUsernameAsync(
        string username,
        CancellationToken cancellationToken);

    Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken);

    Task AddAsync(
        UserEntity user,
        CancellationToken cancellationToken);
}