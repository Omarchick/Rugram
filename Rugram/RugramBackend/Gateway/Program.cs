using Gateway.AutoMapper;
using Gateway.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
	serverOptions.ConfigureEndpointDefaults(options => options.Protocols = HttpProtocols.Http2);
});

builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddCors();
builder.Services.AddGrpc();
builder.Services.AddHttpContextAccessor();

builder.AddSwagger();
builder.AddAuthorization();
builder.AddMasstransitRabbitMq();
builder.AddGrpcClients();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();


if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}


app.UseCors(option =>
{
	option.AllowAnyHeader();
	option.AllowAnyMethod();
	option.AllowAnyOrigin();
});

app.RouteEndpoints();

await Task.Delay(1000 * 20);

app.Run();