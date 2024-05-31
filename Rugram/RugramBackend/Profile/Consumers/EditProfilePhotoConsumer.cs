using Contracts.RabbitMq;
using Infrastructure.S3;
using MassTransit;

namespace Profile.Consumers;

public class EditProfilePhotoConsumer(IS3StorageService s3StorageService)
	: IConsumer<EditProfilePhotoMessage>
{
	public async Task Consume(ConsumeContext<EditProfilePhotoMessage> context)
	{
		await s3StorageService.RemoveFileFromBucketAsync(
			context.Message.ProfileId,
			context.Message.ProfileId,
			context.CancellationToken);

		await s3StorageService.PutFileInBucketAsync(
			new MemoryStream(context.Message.Photo),
			context.Message.ProfileId,
			context.Message.ProfileId,
			context.CancellationToken);
	}
}