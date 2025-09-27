namespace FileStorage.Infrastructure.Storage.Abstractions;

public interface IFileHandler
{
    public Task<bool> SaveFile(FileStream file, string filename, CancellationToken cancellation = default);
    public Task<bool> DeleteFile(string filename, CancellationToken cancellation = default);
    public Task<FileStream> GetFile(string filename, CancellationToken cancellation = default);
}