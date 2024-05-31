using FluentValidation;
using Infrastructure.MediatR.Extensions;
using Infrastructure.S3;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Minio;
using Profile.AutoMapper;
using Profile.Extensions;
using Profile.Grpc.ProfileForAuthService;
using Profile.Grpc.ProfileService;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
	serverOptions.ConfigureEndpointDefaults(options => options.Protocols = HttpProtocols.Http2);
});

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddMediatR(conf => conf.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddValidationBehaviorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddGrpc();
builder.Services.AddSingleton<IS3StorageService, MinioS3StorageService>();
builder.Services.AddMinio(configuration =>
{
	configuration.WithSSL(false);
	configuration.WithTimeout(int.Parse(builder.Configuration["MinioS3:Timeout"]!));
	configuration.WithEndpoint(builder.Configuration["MinioS3:Endpoint"]);
	configuration.WithCredentials(
		builder.Configuration["MinioS3:AccessKey"]!,
		builder.Configuration["MinioS3:SecretKey"]!);
});

builder.AddGrpcClients();
builder.AddMasstransitRabbitMq();
builder.ConfigurePostgresqlConnection();

var app = builder.Build();

await app.MigrateDbAsync();

app.MapGrpcService<ProfileForAuthGrpcService>();
app.MapGrpcService<ProfileGrpcService>();

await Task.Delay(1000 * 20);

app.Run();