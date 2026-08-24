namespace JapaneseLearning.User.Application.Common.Exceptions;

public sealed class ConflictException(
    string code,
    string message)
    : AppException(code, message);