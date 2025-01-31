using System.Net;

namespace Infrastructure.ActionResults;

public readonly record struct Result<T>(string? Error, HttpStatusCode StatusCode, T Value = default(T))
{
    public bool IsSuccess => Error == null;
    
    public void Deconstruct(
        out string? error,
        out HttpStatusCode statusCode,
        out T value)
    {
        error = Error;
        statusCode = StatusCode;
        value = Value;
    }
}