using Auth.Data;
using Infrastructure.MediatR.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Auth.Features.Login;

public class LoginRequestHandler(
		AppDbContext dbContext,
		IConfiguration configuration,
		IDistributedCache cache)
	: IGrpcRequestHandler<LoginRequest, LoginResponse>
{
	public async Task<GrpcResult<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
	{
		var user = await dbContext.Users.AsNoTracking()
			.Select(user => new { user.Id, user.Email, user.Password, user.Role })
			.FirstOrDefaultAsync(user => user.Email == request.Email, cancellationToken);

		if (user == null) return StatusCodes.Status404NotFound;
		if (user.Password != request.Password.HashSha256()) return StatusCodes.Status403Forbidden;

		var jwtToken = UserAuthHelper.GenerateJwtToken(configuration, user.Id, user.Role);
		var result = UserAuthHelper.CreateRefreshToken(configuration, user.Id);

		await UserAuthHelper.PutInCacheRefreshTokenAsync(
			configuration,
			cache,
			result.RefreshToken.Value,
			result.RefreshToken.ValidTo,
			user.Id,
			cancellationToken);

		dbContext.RefreshTokens.Add(result.RefreshToken);
		await dbContext.SaveChangesAsync(cancellationToken);

		return new LoginResponse(jwtToken, result.UnhashedTokenValue);
	}
}