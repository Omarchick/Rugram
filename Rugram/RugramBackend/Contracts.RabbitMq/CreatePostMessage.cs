namespace Contracts.RabbitMq;

public record CreatePostMessage(Guid PostId, Guid UserId, string Description, List<PhotoStream> Photos);

public record PhotoStream(byte[] File);