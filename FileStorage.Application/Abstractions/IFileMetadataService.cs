using FileStorage.Application.Contracts;
using FileStorage.Infrastructure.Database.Entities;

namespace FileStorage.Application.Abstractions;

public interface IFileMetadataService
{
    Task<FileMetadataEntity> SaveMetadata(SaveMetadataDto metadata, CancellationToken cancellation = default);
    Task DeleteMetadata(Guid id, CancellationToken cancellation = default);
    Task<List<FileMetadataEntity>> GetAll(CancellationToken cancellation = default);
    Task<FileMetadataEntity?> GetById(Guid id, CancellationToken cancellation = default);
}