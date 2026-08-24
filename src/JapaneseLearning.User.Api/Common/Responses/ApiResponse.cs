using JapaneseLearning.User.Api.Common.Errors;

namespace JapaneseLearning.User.Api.Common.Responses;

public sealed record ApiResponse<T>(
    bool Success,
    T? Data,
    ApiError? Error,
    string? TraceId)
{
    public static ApiResponse<T> Ok(
        T data,
        string? traceId = null)
        => new(
            Success: true,
            Data: data,
            Error: null,
            TraceId: traceId);

    public static ApiResponse<T> Fail(
        ApiError error,
        string? traceId = null)
        => new(
            Success: false,
            Data: default,
            Error: error,
            TraceId: traceId);
}