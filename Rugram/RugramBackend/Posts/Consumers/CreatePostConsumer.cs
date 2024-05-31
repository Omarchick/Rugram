using Contracts.RabbitMq;
using Infrastructure.S3;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Posts.Data;
using Posts.Data.Models;

namespace Posts.Consumers;

public class CreatePostConsumer(AppDbContext appDbContext, IS3StorageService s3StorageService)
	: IConsumer<CreatePostMessage>
{
	public async Task Consume(ConsumeContext<CreatePostMessage> context)
	{
		if (await appDbContext.Posts.AnyAsync(x => x.Id == context.Message.PostId)) return;

		var post = new Post(context.Message.UserId, context.Message.PostId, context.Message.Description);
		var photos = context.Message.Photos
			.Select(_ => new Photo(context.Message.PostId))
			.ToList();
		post.Photos = photos;

		var transaction = await appDbContext.Database.BeginTransactionAsync(context.CancellationToken);

		appDbContext.Posts.Add(post);
		appDbContext.Photos.AddRange(photos);

		await appDbContext.SaveChangesAsync(context.CancellationToken);

		int? indexSaver = null;

		try
		{
			for (var index = 0; index < photos.Count; index++)
			{
				await s3StorageService.PutFileInBucketAsync(
					new MemoryStream(context.Message.Photos[index].File),
					photos[index].Id,
					context.Message.UserId,
					context.CancellationToken);
				indexSaver = index;
			}

			await transaction.CommitAsync(context.CancellationToken);
		}
		catch (Exception)
		{
			await transaction.RollbackAsync(context.CancellationToken);

			if (!indexSaver.HasValue) throw;

			for (var index = 0; index <= indexSaver; index++)
			{
				await s3StorageService.RemoveFileFromBucketAsync(
					photos[index].Id,
					context.Message.UserId,
					context.CancellationToken);
				indexSaver = index;
			}

			throw;
		}
	}
}