using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auth.Data.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Features;

public static class UserAuthHelper
{
	public static async Task PutInCacheRefreshTokenAsync(
		IConfiguration configuration,
		IDistributedCache cache,
		string refreshToken,
		DateTime validTo,
		Guid userId,
		CancellationToken cancellationToken)
	{
		var slidingExpiration = configuration.GetSlidingExpirationForRefreshToken();

		await cache.SetStringAsync(
			userId.ToString(),
			refreshToken,
			new DistributedCacheEntryOptions()
			{
				AbsoluteExpiration = validTo,
				SlidingExpiration = TimeSpan.FromMinutes(slidingExpiration)
			}, cancellationToken);
	}

	public record CreateRefreshTokenResponse(string UnhashedTokenValue, RefreshToken RefreshToken);

	public static CreateRefreshTokenResponse CreateRefreshToken(
		IConfiguration configuration,
		Guid userId)
	{
		var token = GenerateSecureToken();
		var hashedToken = token.HashSha256();

		var refreshTokenLifetime = int.Parse(configuration["AuthOptions:RefreshTokenLifetimeInHours"]!);

		return new CreateRefreshTokenResponse(
			token,
			new RefreshToken()
			{
				Value = hashedToken,
				ValidTo = DateTime.UtcNow + TimeSpan.FromHours(refreshTokenLifetime),
				UserId = userId
			});
	}

	public static string GenerateJwtToken(
		IConfiguration configuration,
		Guid userId,
		Role role)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var jwtSecurityKey = Encoding.ASCII.GetBytes(
			configuration["AuthOptions:JwtSecretKey"]!);
		var claims = new List<Claim>
		{
			new(nameof(ClaimTypes.NameIdentifier), userId.ToString()),
			new(nameof(ClaimTypes.Role), ((int)role).ToString())
		};

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Audience = configuration["AuthOptions:Audience"],
			Issuer = configuration["AuthOptions:Issuer"],
			Subject = new ClaimsIdentity(claims.ToArray()),
			Expires = DateTime.UtcNow.AddMinutes(int.Parse(
				configuration["AuthOptions:AccessTokenLifetimeInMinutes"]!)),
			SigningCredentials =
				new SigningCredentials(new SymmetricSecurityKey(jwtSecurityKey),
					SecurityAlgorithms.HmacSha256Signature)
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(token);
	}

	public static string GenerateSecureToken()
	{
		var randomNumber = new byte[32];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);

		return HashSha256(Convert.ToBase64String(randomNumber));
	}

	public static string HashSha256(this string inputString)
	{
		var inputBytes = Encoding.UTF32.GetBytes(inputString);
		var hashedBytes = SHA256.HashData(inputBytes);

		return Convert.ToBase64String(hashedBytes);
	}

	private static int GetSlidingExpirationForRefreshToken(this IConfiguration configuration)
	{
		return int.Parse(configuration["Cache:SlidingExpirationForRefreshTokenInMinutes"]!);
	}
}