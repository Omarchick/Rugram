using Contracts.RabbitMq;
using Infrastructure.S3;
using MassTransit;

namespace Posts.Consumers;

public class DeleteBucketConsumer(IS3StorageService s3StorageService) : IConsumer<DeleteBucketMessage>
{
	public async Task Consume(ConsumeContext<DeleteBucketMessage> context)
	{
		if (await s3StorageService.BucketExistAsync(context.Message.BucketIdentifier, context.CancellationToken))
			await s3StorageService.RemoveBucketAsync(context.Message.BucketIdentifier, context.CancellationToken);
	}
}