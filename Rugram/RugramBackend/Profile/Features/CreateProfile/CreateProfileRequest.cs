using Infrastructure.MediatR.Contracts;

namespace Profile.Features.CreateProfile;

public record CreateProfileRequest(Guid ProfileId, string ProfileName) : IGrpcRequest;