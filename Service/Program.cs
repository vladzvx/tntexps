using Microsoft.OpenApi.Models;
Thread.Sleep(5000);
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();
services.AddSwaggerGen().AddSwaggerGenNewtonsoftSupport();

services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.ShowCommonExtensions();
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TarantoolExperiments");
});

app.MapControllers();

app.Run();
