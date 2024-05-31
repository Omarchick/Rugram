using Infrastructure.MediatR.Contracts;

namespace Posts.Features.GetPhoto;

public record GetPhotoRequest(Guid ProfileId, Guid PhotoId) : IGrpcRequest<GetPhotoResponse>;