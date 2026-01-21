using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuração simplificada para evitar o erro CS0234
builder.Services.AddSwaggerGen(); 

// Conexão para DEVELOPER02-NB\MSSQLSERVER2019
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new System.InvalidOperationException("Connection string não encontrada.");

builder.Services.AddSingleton(connectionString);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();