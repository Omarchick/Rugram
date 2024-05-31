using Infrastructure.MediatR.Contracts;

namespace Profile.Features.GetProfilePhoto;

public record GetProfilePhotoRequest(Guid ProfileId) : IGrpcRequest<GetProfilePhotoResponse>;