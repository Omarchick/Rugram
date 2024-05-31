using FluentValidation;
using Infrastructure.MediatR.Extensions;
using Infrastructure.S3;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Minio;
using Posts.AutoMapper;
using Posts.Extensions;
using Posts.Grpc.PostForProfileService;
using Posts.Grpc.PostService;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
	serverOptions.ConfigureEndpointDefaults(options => options.Protocols = HttpProtocols.Http2);
});

builder.ConfigurePostgresqlConnection();
builder.AddMasstransitRabbitMq();

builder.Services.AddGrpc();
builder.Services.AddSingleton<IS3StorageService, MinioS3StorageService>();
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddValidationBehaviorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddMediatR(conf => conf.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddMinio(configuration =>
{
	configuration.WithSSL(false);
	configuration.WithTimeout(int.Parse(builder.Configuration["MinioS3:Timeout"]!));
	configuration.WithEndpoint(builder.Configuration["MinioS3:Endpoint"]);
	configuration.WithCredentials(
		builder.Configuration["MinioS3:AccessKey"]!,
		builder.Configuration["MinioS3:SecretKey"]!);
});

var app = builder.Build();

await app.MigrateDbAsync();

app.MapGrpcService<PostGrpcService>();
app.MapGrpcService<PostForProfileService>();

await Task.Delay(1000 * 20);

app.Run();