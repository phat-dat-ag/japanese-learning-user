using JapaneseLearning.User.Domain.Users;

namespace JapaneseLearning.User.Domain.Entities;

public sealed class User
{
    public Guid Id { get; private set; }

    public UserRole Role { get; private set; }

    public string Username { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public string PasswordHash { get; private set; } = null!;

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    private User()
    {
    }

    public User(
        Guid id,
        string username,
        string email,
        string passwordHash,
        UserRole role,
        bool isActive,
        DateTime createdAt)
    {
        Id = id;
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = null;
    }
}