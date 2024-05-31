using Contracts.RabbitMq;
using Infrastructure.S3;
using MassTransit;

namespace Posts.Consumers;

public class CreateBucketConsumer(IS3StorageService s3StorageService) : IConsumer<CreateBucketMessage>
{
	public async Task Consume(ConsumeContext<CreateBucketMessage> context)
	{
		if (!await s3StorageService.BucketExistAsync(context.Message.BucketIdentifier, context.CancellationToken))
			await s3StorageService.CreateBucketAsync(context.Message.BucketIdentifier, context.CancellationToken);
	}
}