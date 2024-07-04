using DotnetTemplate.Application.Interfaces;
using DotnetTemplate.Application.Services;
using DotnetTemplate.EntityFramework.Shared.DbContexts;
using DotnetTemplate.Infraestructure.Interfaces;
using DotnetTemplate.Infraestructure.Repository;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexi�n desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("MariaDbConnection");
// Configurar el DbContext con MariaDB
builder.Services.AddDbContext<DotnetTemplateDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
//Add service
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
//Add Repository
builder.Services.AddScoped<IAuditService, AuditService>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy",
                      builder =>
                      {
                          builder.SetIsOriginAllowed((host) => true)
                                 .AllowAnyMethod()
                                 .AllowAnyHeader()
                                 .AllowCredentials();
                      });
});

builder.Services.AddOpenTelemetry()
    .WithMetrics(x => {
        x.AddPrometheusExporter();
        x.AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel");
        x.AddView("request-duration", new ExplicitBucketHistogramConfiguration
        { 
            Boundaries = new[] { 0,0.005,0.01,0.025,0.05,0.075,0.1,0.25,0.5,0.7}
        });
    });

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configurar la tubería HTTP
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseMetricServer();

app.UseHealthChecks("/health", new HealthCheckOptions
{
  Predicate = _ => false
});
app.UseHealthChecks("/health/ready", new HealthCheckOptions
{
  Predicate = healthCheck => healthCheck.Tags.Contains("ready")
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors("CorsPolicy"); // Use the CORS policy in development environment
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseHttpMetrics(options =>
{
    options.AddCustomLabel("host", context => context.Request.Host.Host);
});

app.MapPrometheusScrapingEndpoint();

app.UseAuthorization();

app.MapControllers();

app.Run();