/*
public class KeyMiddleware
{
    private const string KeyName = "x-api-key";
    private const string ClientName = "x-client-name";

    private readonly RequestDelegate _next;

    public KeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey(KeyName) || !context.Request.Headers.ContainsKey(ClientName))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Chave não informada");
            return;
        }

        string client = context.Request.Headers[ClientName];
        string DesiredKey = context.RequestServices.GetRequiredService<IConfiguration>().GetSection("Keys").GetValue<string>(client);

        if (DesiredKey == null || DesiredKey != context.Request.Headers[KeyName])
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Acesso não autorizado");
            return;
        }

        await _next(context);
    }
}

public static class KeyMiddlewareExtensions
{
    public static IApplicationBuilder UseAPIKey(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<KeyMiddleware>();
    }
}
*/