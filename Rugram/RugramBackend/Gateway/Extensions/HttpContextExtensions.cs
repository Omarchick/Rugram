using System.Security.Claims;

namespace Gateway.Extensions;

public static class HttpContextExtensions
{
	/// <summary>
	/// Получить id пользователя из httpContext
	/// </summary>
	/// <param name="httpContext">HttpContext</param>
	/// <returns>id пользователя</returns>
	/// <exception cref="ArgumentException">Пользователь не авторизирован или нет клэйма
	/// с типом ClaimTypes.NameIdentifier</exception>
	public static Guid GetUserId(this HttpContext httpContext)
	{
		if (httpContext.User.Identity is { IsAuthenticated: false })
			throw new ArgumentException("Пользователь не авторизован");

		var claim = httpContext.User.Claims
			            .FirstOrDefault(claim => claim.Type == nameof(ClaimTypes.NameIdentifier))
		            ?? throw new ArgumentException(
			            $"У зарегистрированного пользователя нет клэйма: '{nameof(ClaimTypes.NameIdentifier)}'");

		return new Guid(claim.Value);
	}
}