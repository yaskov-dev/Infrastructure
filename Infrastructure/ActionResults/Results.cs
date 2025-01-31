using System.Net;

namespace Infrastructure.ActionResults;

public static class Results
{
    public static Result<T> Ok<T>(T value) => new(null, HttpStatusCode.OK, value);

    public static Result<T> NoContent<T>() => new(null, HttpStatusCode.NoContent);
    public static Result<T> BadRequest<T>(string e) => new(e, HttpStatusCode.BadRequest);
    public static Result<T> NotFound<T>(string e) => new(e, HttpStatusCode.NotFound);
    public static Result<T> InternalServerError<T>(string e) => new(e, HttpStatusCode.InternalServerError);


    public static Result<T2> ErrorFrom<T, T2>(Result<T> source)
    {
        if (source.IsSuccess)
            throw new ArgumentException("source Result should be unsuccessful");
        
        return new Result<T2>(source.Error, source.StatusCode);
    }

    public static Result<T> ErrorFromHttp<T>(Result<HttpResponseMessage> source)
        => ErrorFrom<HttpResponseMessage, T>(source);
    
    public static Result<T> ErrorFromHttp<T>(HttpResponseMessage response)
    {
        var errorMessage = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        return response.StatusCode switch
        {
            HttpStatusCode.BadRequest => BadRequest<T>(errorMessage),
            HttpStatusCode.NotFound => NotFound<T>(errorMessage),
            HttpStatusCode.InternalServerError => InternalServerError<T>(errorMessage),
            _ => throw new NotImplementedException($"Result does not support error: {response.StatusCode}")
        };
    }
}