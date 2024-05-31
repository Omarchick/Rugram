using Auth.AutoMapper;
using Auth.Extensions;
using Auth.Grpc.AuthService;
using Auth.Services.BackgroundServices;
using Auth.Services.Infrastructure.EmailSenderService;
using FluentValidation;
using Infrastructure.MediatR.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
	serverOptions.ConfigureEndpointDefaults(options => options.Protocols = HttpProtocols.Http2);
});

builder.Configuration.AddEnvironmentVariables();

builder.ConfigurePostgresqlConnection();
builder.ConfigureRedisConnection();
builder.AddGrpcClients();
builder.AddMasstransitRabbitMq();

builder.Services.AddMediatR(conf => conf.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddValidationBehaviorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddGrpc();
builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddSingleton<IEmailSenderService, EmailSenderService>();

builder.Services.AddHostedService<DeleteOutdatedRefreshTokens>();
builder.Services.AddHostedService<DeleteOutdatedMailConfirmationTokens>();

var app = builder.Build();

await app.MigrateDbAsync();

app.MapGrpcService<AuthGrpcService>();

await Task.Delay(1000 * 20);

app.Run();