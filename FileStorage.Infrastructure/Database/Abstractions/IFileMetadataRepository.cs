using FileStorage.Infrastructure.Database.Entities;

namespace FileStorage.Infrastructure.Database.Abstractions;

public interface IFileMetadataRepository
{
    Task<FileMetadataEntity> SaveMetadata(FileMetadataEntity metadata, CancellationToken cancellation = default);
    Task DeleteMetadata(Guid id, CancellationToken cancellation = default);
    Task<List<FileMetadataEntity>> GetAll(CancellationToken cancellation = default);
    Task<FileMetadataEntity?> GetById(Guid id, CancellationToken cancellation = default);
}