using Minio.Exceptions;

namespace Infrastructure.S3;

/// <summary>
/// Контракт для работы с S3
/// </summary>
public interface IS3StorageService
{
	/// <summary>
	/// Проверить существует ли бакет 
	/// </summary>
	/// <param name="bucketIdentifier">уникальный идентификатор бакета</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>true если существует</returns>
	public Task<bool> BucketExistAsync(Guid bucketIdentifier, CancellationToken cancellationToken);

	/// <summary>
	/// Созать бакет
	/// </summary>
	/// <param name="bucketIdentifier">уникальный идентификатор бакета</param>
	/// <param name="cancellationToken">Токен отмены</param>
	public Task CreateBucketAsync(Guid bucketIdentifier, CancellationToken cancellationToken);

	/// <summary>
	/// Удалить бакет
	/// </summary>
	/// <param name="bucketIdentifier">уникальный идентификатор бакета</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns></returns>
	public Task RemoveBucketAsync(Guid bucketIdentifier, CancellationToken cancellationToken);

	/// <summary>
	/// Положить файл в бакет
	/// </summary>
	/// <param name="fileStream">Файл в виде <see cref="Stream"/></param>
	/// <param name="fileIdentifier">уникальный идентификатор файла</param>
	/// <param name="bucketIdentifier">уникальный идентификатор бакета</param>
	/// <param name="cancellationToken">Токен отмены</param>
	public Task PutFileInBucketAsync(
		Stream fileStream,
		Guid fileIdentifier,
		Guid bucketIdentifier,
		CancellationToken cancellationToken);

	/// <summary>
	/// Удалить файл из бакета
	/// </summary>
	/// <param name="fileIdentifier">уникальный идентификатор файла</param>
	/// <param name="bucketIdentifier">уникальный идентификатор бакета</param>
	/// <param name="cancellationToken">Токен отмены</param>
	public Task RemoveFileFromBucketAsync(Guid fileIdentifier,
		Guid bucketIdentifier,
		CancellationToken cancellationToken);

	/// <summary>
	/// Получить файл из бакета
	/// </summary>
	/// <param name="fileIdentifier">уникальный идентификатор файла</param>
	/// <param name="bucketIdentifier">уникальный идентификатор бакета</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>Файл в виде <see cref="MemoryStream"/></returns>
	/// <exception cref="BucketNotFoundException">бакет не найден</exception>
	/// <exception cref="ObjectNotFoundException">файл не найден</exception>
	public Task<MemoryStream> GetFileFromBucketAsync(
		Guid fileIdentifier,
		Guid bucketIdentifier,
		CancellationToken cancellationToken);
}