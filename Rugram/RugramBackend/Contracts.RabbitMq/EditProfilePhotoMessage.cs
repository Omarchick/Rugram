namespace Contracts.RabbitMq;

public record EditProfilePhotoMessage(Guid ProfileId, byte[] Photo);