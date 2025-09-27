namespace FileStorage.Application.Abstractions;

public interface IFileService
{
    public Task<bool> SaveFile(FileStream file, CancellationToken cancellation = default);
    public Task<bool> DeleteFile(Guid id, CancellationToken cancellation = default);
    public Task<FileStream> GetFile(Guid id, CancellationToken cancellation = default);
}