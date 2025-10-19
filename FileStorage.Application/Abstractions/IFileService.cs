using FileStorage.Application.Contracts;
using FileStorage.Infrastructure;

namespace FileStorage.Application.Abstractions;

public interface IFileService
{
    public Task<Result<Guid>> SaveFile(Stream file, CancellationToken cancellation = default);
    public Task<Result> DeleteFile(Guid id, CancellationToken cancellation = default);
    public Task<Result<FileStream>> GetFile(Guid id, CancellationToken cancellation = default);
}