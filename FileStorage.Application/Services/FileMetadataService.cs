using FileStorage.Application.Abstractions;
using FileStorage.Application.Contracts;
using FileStorage.Infrastructure.Database.Abstractions;
using FileStorage.Infrastructure.Database.Entities;

namespace FileStorage.Application.Services;

public class FileMetadataService : IFileMetadataService
{
    private readonly IFileMetadataRepository _repository;

    public FileMetadataService(IFileMetadataRepository repository)
    {
        _repository = repository;
    }

    public async Task<FileMetadataEntity> SaveMetadata(SaveMetadataDto metadata, CancellationToken cancellation = default)
    {
        try
        {
            var metaEntity = new FileMetadataEntity
            {
                Id = metadata.Id,
                FileName = metadata.FileName,
                Extension = metadata.Extension,
                Size = metadata.Size,
                UploadDate = metadata.UploadDate
            };

            return await _repository.SaveMetadata(metaEntity, cancellation);
        }
        catch
        {
            return await Task.FromResult<FileMetadataEntity>(null!);
        }
    }

    public Task DeleteMetadata(Guid id, CancellationToken cancellation = default)
    {
        try
        {
            return _repository.DeleteMetadata(id, cancellation);
        }
        catch
        {
            return Task.FromResult<FileMetadataEntity>(null!);
        }
    }

    public async Task<List<FileMetadataEntity>> GetAll(CancellationToken cancellation = default)
    {
        try
        {
            return await _repository.GetAll(cancellation);
        }
        catch
        {
            return await Task.FromResult<List<FileMetadataEntity>>(null!);
        }
    }

    public async Task<FileMetadataEntity?> GetById(Guid id, CancellationToken cancellation = default)
    {
        try
        {
            return await _repository.GetById(id, cancellation);
        }
        catch
        {
            return await Task.FromResult<FileMetadataEntity>(null!);
        }
    }
}