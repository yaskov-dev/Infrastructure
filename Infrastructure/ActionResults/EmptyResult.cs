using System.Net;

namespace Infrastructure.ActionResults;

public readonly record struct EmptyResult(string? Error, HttpStatusCode StatusCode)
{
    public bool IsSuccess => Error == null;

    public void Deconstruct(
        out string? error,
        out HttpStatusCode statusCode)
    {
        error = Error;
        statusCode = StatusCode;
    }
}