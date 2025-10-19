namespace FileStorage.Infrastructure.Storage.Abstractions;

public interface IFileHandler
{
    public Task<Result<Guid>> SaveFile(Stream file, CancellationToken cancellation = default);
    public Task<Result> DeleteFile(string id, CancellationToken cancellation = default);
    public Task<Result<FileStream>> GetFile(string id, CancellationToken cancellation = default);
}