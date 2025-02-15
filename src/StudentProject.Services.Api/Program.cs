using StudentProject.Infra.Data.Context;
using StudentProject.Services.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddLoggerConfig();
builder.Services.AddApiConfig(builder.Configuration);
builder.Services.AddDataConfigurations();
builder.Services.AddSwaggerConfig();
builder.Services.ResolveDependencies(builder.Configuration);
builder.Services.AddHealthConfig(builder.Configuration);

var app = builder.Build();

app.UseHealthConfig();
app.UseApiConfiguration(app.Environment);
app.UseSwaggerConfig();
app.Run();