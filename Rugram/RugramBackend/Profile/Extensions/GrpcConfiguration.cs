namespace Profile.Extensions;
using static PostForProfileMicroservice.PostForProfileMicroservice;

public static class GrpcConfiguration
{
	/// <summary>
	/// Добавление Grpc клиентов в сервис провайдер 
	/// </summary>
	/// <param name="builder">IWebHostBuilder</param>
	public static void AddGrpcClients(this WebApplicationBuilder builder)
	{
		builder.Services.AddGrpcClient<PostForProfileMicroserviceClient>(conf =>
		{
			conf.Address = new Uri(builder.Configuration["Microservices:PostAddress"]
			                       ?? throw new ApplicationException(
				                       "Enviroment variable Microservices:PostAddress not found "));
		});
	}
}