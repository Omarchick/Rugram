using Infrastructure.MediatR.Contracts;

namespace Profile.Features.Unsubscribe;

public record UnsubscribeRequest
	(Guid SubscriberId, Guid IdOfProfileUnsubscribedTo) : IGrpcRequest;