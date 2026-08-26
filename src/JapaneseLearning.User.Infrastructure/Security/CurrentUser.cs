using System.Security.Claims;
using JapaneseLearning.User.Application.Abstractions.Security;
using Microsoft.AspNetCore.Http;

namespace JapaneseLearning.User.Infrastructure.Security;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal User =>
        _httpContextAccessor.HttpContext?.User
        ?? new ClaimsPrincipal();

    public bool IsAuthenticated =>
        User.Identity?.IsAuthenticated ?? false;

    public Guid? UserId
    {
        get
        {
            var value = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            return Guid.TryParse(value, out var id)
                ? id
                : null;
        }
    }

    public string? Username =>
        User.FindFirstValue(
            ClaimTypes.Name);

    public string? Email =>
        User.FindFirstValue(
            ClaimTypes.Email);

    public string? Role =>
        User.FindFirstValue(
            ClaimTypes.Role);
}