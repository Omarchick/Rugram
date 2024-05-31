namespace Auth.Extensions;

public static class GrpcConfiguration
{
	/// <summary>
	/// Добавление Grpc клиентов в сервис провайдер 
	/// </summary>
	/// <param name="builder">IWebHostBuilder</param>
	public static void AddGrpcClients(this WebApplicationBuilder builder)
	{
		builder.Services.AddGrpcClient<ProfileForAuthMicroservice.ProfileForAuthMicroserviceClient>(conf =>
		{
			conf.Address = new Uri(builder.Configuration["Microservices:ProfileAddress"]
			                       ?? throw new ApplicationException(
				                       "Enviroment variable Microservices:ProfileAddress not found "));
		});
	}
}