using Gateway.Contracts;

namespace Gateway.Extensions;

public static class EndpointsConfiguration
{
	/// <summary>
	/// Роут всех реализаций интерфейса IEndpoint
	/// </summary>
	/// <param name="app">WebApplication</param>
	public static void RouteEndpoints(this IEndpointRouteBuilder app)
	{
		var endpoints = typeof(Program).Assembly.GetTypes()
			.Where(type => type.IsAssignableTo(typeof(IEndpoint)) &&
			               type is { IsAbstract: false, IsInterface: false });

		foreach (var endpoint in endpoints)
		{
			((IEndpoint)Activator.CreateInstance(endpoint)!).AddRoute(app);
		}
	}
}