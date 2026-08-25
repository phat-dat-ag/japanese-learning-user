namespace JapaneseLearning.User.Application.Common.Exceptions;

public sealed class NotFoundException(
    string code,
    string message)
    : AppException(code, message);