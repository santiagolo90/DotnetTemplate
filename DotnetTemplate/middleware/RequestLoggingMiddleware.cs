using Microsoft.AspNetCore.Http;
using DotnetTemplate.Application.Interfaces;
using DotnetTemplate.EntityFramework.Shared.Entities;
using DotnetTemplate.EntityFramework.Shared.DbContexts;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    const string ORIGIN = "Dotnet";
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public RequestLoggingMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        LogRequest(context);
        await _next(context);
    }

    private async void LogRequest(HttpContext context)
    {
        // Obtiene la dirección IP del cliente
        var ipAddress = context?.Connection?.RemoteIpAddress?.ToString();
        var method = context?.Request?.Method?.ToString();
        var route = context?.Request?.Path.ToString();
        
        AuditDto auditDto = new AuditDto
        {
            Route = route,
            Method = method,
            Ip = ipAddress,
            Origin = ORIGIN
        };
        //swagger/index.html
        if (route != null && !route.Contains("swagger") && !route.Contains("metrics"))
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DotnetTemplateDbContext>();
                await dbContext.Audit.AddAsync(auditDto);
                await dbContext.SaveChangesAsync();
            }
        }
        
    }
}