using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.S3;

public class MinioS3StorageService(IMinioClient minioClient) : IS3StorageService
{
	public async Task<bool> BucketExistAsync(Guid bucketIdentifier, CancellationToken cancellationToken)
	{
		var args = new BucketExistsArgs()
			.WithBucket(bucketIdentifier.ToString());

		return await minioClient.BucketExistsAsync(args, cancellationToken);
	}

	public async Task CreateBucketAsync(Guid bucketIdentifier, CancellationToken cancellationToken)
	{
		var args = new MakeBucketArgs()
			.WithBucket(bucketIdentifier.ToString());

		await minioClient.MakeBucketAsync(args, cancellationToken);
	}

	public async Task RemoveBucketAsync(Guid bucketIdentifier, CancellationToken cancellationToken)
	{
		var removeBucketArgs = new RemoveBucketArgs()
			.WithBucket(bucketIdentifier.ToString());

		await minioClient.RemoveBucketAsync(removeBucketArgs, cancellationToken);
	}

	public async Task PutFileInBucketAsync(
		Stream fileStream,
		Guid fileIdentifier,
		Guid bucketIdentifier,
		CancellationToken cancellationToken)
	{
		var args = new PutObjectArgs()
			.WithBucket(bucketIdentifier.ToString())
			.WithStreamData(fileStream)
			.WithObject(fileIdentifier.ToString())
			.WithObjectSize(fileStream.Length);

		await minioClient.PutObjectAsync(args, cancellationToken);
	}

	public async Task RemoveFileFromBucketAsync(Guid fileIdentifier,
		Guid bucketIdentifier,
		CancellationToken cancellationToken)
	{
		var args = new RemoveObjectArgs()
			.WithBucket(bucketIdentifier.ToString())
			.WithObject(fileIdentifier.ToString());

		await minioClient.RemoveObjectAsync(args, cancellationToken);
	}

	public async Task<MemoryStream> GetFileFromBucketAsync(
		Guid fileIdentifier,
		Guid bucketIdentifier,
		CancellationToken cancellationToken)
	{
		var response = new MemoryStream();
		var args = new GetObjectArgs()
			.WithBucket(bucketIdentifier.ToString())
			.WithObject(fileIdentifier.ToString())
			.WithCallbackStream(stream => stream.CopyTo(response));

		await minioClient.GetObjectAsync(args, cancellationToken);

		response.Position = 0;
		return response;
	}
}