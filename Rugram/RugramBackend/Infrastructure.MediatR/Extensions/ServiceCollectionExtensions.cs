using System.Reflection;
using FluentValidation;
using Infrastructure.MediatR.Behaviors;
using Infrastructure.MediatR.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.MediatR.Extensions;

public static class ServiceCollectionExtensions
{
	private static readonly Type RequestWithResponseType = typeof(IGrpcRequest<>);
	private static readonly Type PipelineBehaviorType = typeof(IPipelineBehavior<,>);
	private static readonly Type GenericResultType = typeof(GrpcResult<>);
	private static readonly Type ResultType = typeof(GrpcResult);

	/// <summary>
	/// Добавление ValidationBehavior для всех запросов в сброке имеющих Validator : AbstractValidator
	/// в IServiceCollection
	/// </summary>
	/// <param name="serviceCollection">IServiceCollection</param>
	/// <param name="assembly">Assembly</param>
	public static void AddValidationBehaviorsFromAssembly(
		this IServiceCollection serviceCollection,
		Assembly assembly)
	{
		var requests = assembly.GetTypes()
			.Where(type => IsAssignableToGenericType(type, typeof(IValidator<>)) &&
			               type is { IsClass: true, IsAbstract: false })
			.Select(type => type.BaseType!.GetGenericArguments()[0])
			.ToList();

		var responses = requests
			.Select(request => request.GetInterface(RequestWithResponseType.Name) is null
				? ResultType
				: request.GetInterface(RequestWithResponseType.Name)!.GetGenericArguments()[0])
			.ToList();

		for (var i = 0; i < requests.Count; i++)
		{
			var validationBehaviorType = responses[i] == ResultType
				? typeof(ValidationBehavior<>)
				: typeof(ValidationBehavior<,>);
			var genericResultType = responses[i] == ResultType
				? ResultType
				: GenericResultType.MakeGenericType(responses[i]);
			var genericPipelineBehaviorType = PipelineBehaviorType.MakeGenericType(requests[i], genericResultType);
			var genericValidationBehaviorType = responses[i] == ResultType
				? validationBehaviorType.MakeGenericType(requests[i])
				: validationBehaviorType.MakeGenericType(requests[i], responses[i]);

			serviceCollection.AddTransient(
				genericPipelineBehaviorType,
				genericValidationBehaviorType);
		}
	}

	private static bool IsAssignableToGenericType(Type givenType, Type genericType)
	{
		var interfaceTypes = givenType.GetInterfaces();

		if (interfaceTypes.Any(type => type.IsGenericType && type.GetGenericTypeDefinition() == genericType))
		{
			return true;
		}

		if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
		{
			return true;
		}

		var baseType = givenType.BaseType;

		return baseType != null && IsAssignableToGenericType(baseType, genericType);
	}
}