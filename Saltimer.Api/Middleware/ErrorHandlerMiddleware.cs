using System.Net;
using System.Text.Json;


namespace Saltimer.Api.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (HttpRequestException error)
        {
            var response = context.Response;

            response.ContentType = "application/json";
            var errorStatus = (error.StatusCode is not null) ? (HttpStatusCode)error.StatusCode : HttpStatusCode.InternalServerError;
            response.StatusCode = (int)errorStatus;

            var result = JsonSerializer.Serialize(new { message = error?.Message, Status = HttpStatusCode.GetName(errorStatus) });
            await response.WriteAsync(result);
        }
        catch (Exception error)
        {
            var response = context.Response;

            response.ContentType = "application/json";
            var errorStatus = HttpStatusCode.InternalServerError;

            var result = JsonSerializer.Serialize(new { message = error?.Message, Status = HttpStatusCode.GetName(errorStatus) });
            await response.WriteAsync(result);
        }
    }
}