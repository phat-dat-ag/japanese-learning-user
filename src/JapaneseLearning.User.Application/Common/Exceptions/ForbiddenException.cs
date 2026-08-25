namespace JapaneseLearning.User.Application.Common.Exceptions;

public sealed class ForbiddenException(
    string code,
    string message)
    : AppException(code, message);