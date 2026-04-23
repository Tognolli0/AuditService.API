using AuditService.API.Data;
using AuditService.API.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Configure ConnectionStrings:DefaultConnection antes de iniciar a API.");
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AuditService.API",
        Version = "v1",
        Description = "Servico para registro, consulta e rastreabilidade de eventos de auditoria."
    });
});

builder.Services.AddScoped<IAuditEventRepository>(_ => new SqlAuditEventRepository(connectionString));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.DocumentTitle = "AuditService.API Docs";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AuditService.API v1");
});

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseAuthorization();
app.MapControllers();
app.MapGet("/healthz", () => Results.Ok(new
{
    status = "healthy",
    time = DateTime.UtcNow
}))
.WithName("HealthCheck")
.WithSummary("Verifica a disponibilidade basica da API");

app.Run();
