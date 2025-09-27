using FileStorage.Application.Abstractions;
using FileStorage.Application.Contracts;
using FileStorage.Infrastructure.Storage.Abstractions;

namespace FileStorage.Application.Services;

public class FileService : IFileService
{
    private readonly IFileMetadataService _metaService;
    private readonly IFileHandler _fileHandler;

    public FileService(IFileMetadataService metaService, IFileHandler fileHandler)
    {
        _metaService = metaService;
        _fileHandler = fileHandler;
    }

    public async Task<bool> SaveFile(FileStream file, CancellationToken cancellation = default)
    {
        try
        {
            var fileMetadata = new SaveMetadataDto()
            {
                Id = Guid.NewGuid(),
                FileName = file.Name,
                Extension = Path.GetExtension(file.Name),
                Size = file.Length,
                UploadDate = DateTime.Now
            };
            var createMetaResult = await _metaService.SaveMetadata(fileMetadata, cancellation);

            var result = await _fileHandler.SaveFile(file, createMetaResult.Id.ToString(), cancellation);

            if (!result)
                await _metaService.DeleteMetadata(createMetaResult.Id, cancellation);

            return result;
        }
        catch
        {
            return false;
        }

    }

    public async Task<bool> DeleteFile(Guid id, CancellationToken cancellation = default)
    {
        try
        {
            var meta = await _metaService.GetById(id, cancellation);

            if (meta is null)
                return false;

            var deleteFileResult = await _fileHandler.DeleteFile($"{meta.Id}.{meta.Extension}", cancellation);

            if (!deleteFileResult)
                return false;

            await _metaService.DeleteMetadata(meta.Id, cancellation);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<FileStream> GetFile(Guid id, CancellationToken cancellation = default)
    {
        try
        {
            var meta = await _metaService.GetById(id, cancellation);
            if (meta is null)
                return await Task.FromResult<FileStream>(null!);

            var file = await _fileHandler.GetFile($"{meta.Id}.{meta.Extension}", cancellation);

            return file;
        }
        catch
        {
            return await Task.FromResult<FileStream>(null!);
        }
    }
}