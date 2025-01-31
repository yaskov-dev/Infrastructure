#nullable enable

using System.Net;

namespace Infrastructure.ActionResults;

public static class EmptyResults
{
    public static EmptyResult NoContent() => new(null, HttpStatusCode.NoContent);
    public static EmptyResult BadRequest(string e) => new(e, HttpStatusCode.BadRequest);
    public static EmptyResult NotFound(string e) => new(e, HttpStatusCode.NotFound);
    public static EmptyResult InternalServerError(string e) => new(e, HttpStatusCode.InternalServerError);

    public static Result<T2> ErrorFrom<T, T2>(EmptyResult source)
    {
        if (source.IsSuccess)
            throw new ArgumentException("source Result should be unsuccessful");
        
        return new Result<T2>(source.Error, source.StatusCode);
    }

    public static EmptyResult ErrorFromHttp(HttpResponseMessage response)
    {
        var errorMessage = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        return response.StatusCode switch
        {
            HttpStatusCode.BadRequest => BadRequest(errorMessage),
            HttpStatusCode.NotFound => NotFound(errorMessage),
            HttpStatusCode.InternalServerError => InternalServerError(errorMessage),
            _ => throw new NotImplementedException($"Result does not support error: {response.StatusCode}")
        };
    }
}