using JapaneseLearning.User.Application.Abstractions.Persistence;
using JapaneseLearning.User.Application.Abstractions.Security;
using JapaneseLearning.User.Application.Common.Exceptions;
using UserEntity = JapaneseLearning.User.Domain.Entities.User;
using JapaneseLearning.User.Domain.Users;
using MediatR;

namespace JapaneseLearning.User.Application.Auth.Register;

public sealed class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterResponse> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var username = request.Username.Trim();

        var email = request.Email
            .Trim()
            .ToLowerInvariant();

        var usernameExists =
            await _userRepository.ExistsByUsernameAsync(
                username,
                cancellationToken);

        if (usernameExists)
        {
            throw new ConflictException(
                "USERNAME_ALREADY_EXISTS",
                "Username is already registered.");
        }

        var emailExists =
            await _userRepository.ExistsByEmailAsync(
                email,
                cancellationToken);

        if (emailExists)
        {
            throw new ConflictException(
                "EMAIL_ALREADY_EXISTS",
                "Email is already registered.");
        }

        var passwordHash =
            _passwordHasher.Hash(request.Password);

        var now = DateTime.UtcNow;

        var user = new UserEntity(
            Guid.NewGuid(),
            username,
            email,
            passwordHash,
            UserRole.User,
            true,
            now);

        await _userRepository.AddAsync(
            user,
            cancellationToken);

        return new RegisterResponse(
            user.Id,
            user.Username,
            user.Email,
            user.CreatedAt);
    }
}