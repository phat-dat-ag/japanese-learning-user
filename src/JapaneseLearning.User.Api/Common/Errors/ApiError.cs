namespace JapaneseLearning.User.Api.Common.Errors;

public sealed record ApiError(
    string Code,
    string Message,
    IReadOnlyCollection<ApiErrorDetail>? Details = null);