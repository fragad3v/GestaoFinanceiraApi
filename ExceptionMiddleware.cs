using System.Net;
using System.Text.Json;

namespace GestaoFinanceiraApi;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var erro = new { mensagem = ex.Message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(erro));
        }
    }
}
