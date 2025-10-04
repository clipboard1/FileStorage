using FileStorage.Application.Contracts;

namespace FileStorage.Application.Abstractions;

public interface IFileService
{
    public Task<Guid> SaveFile(Stream file, string fileName, CancellationToken cancellation = default);
    public Task<bool> DeleteFile(string id, CancellationToken cancellation = default);
    public Task<DownloadFileDTO> GetFile(string id, CancellationToken cancellation = default);
    public Task<List<FileInfoDTO>> GetAllFiles(CancellationToken cancellation = default);
}