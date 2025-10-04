using FileStorage.Application.Abstractions;
using FileStorage.Application.Contracts;
using FileStorage.Infrastructure.Database.Entities;
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

    public async Task<Guid> SaveFile(Stream file, string fileName, CancellationToken cancellation = default)
    {
        try
        {
            var fileMetadata = new SaveMetadataDto()
            {
                Id = Guid.NewGuid(),
                FileName = Path.GetFileNameWithoutExtension(fileName),
                Extension = Path.GetExtension(fileName),
                Size = file.Length,
                UploadDate = DateTime.UtcNow
            };
            var createMetaResult = await _metaService.SaveMetadata(fileMetadata, cancellation);

            var result = await _fileHandler.SaveFile(
                file,
                $"{createMetaResult.Id.ToString()}{createMetaResult.Extension}",
                cancellation);

            if (!result)
                await _metaService.DeleteMetadata(createMetaResult.Id, cancellation);

            return createMetaResult.Id;
        }
        catch
        {
            return Guid.Empty;
        }

    }

    public async Task<bool> DeleteFile(string id, CancellationToken cancellation = default)
    {
        try
        {
            var meta = await _metaService.GetById(Guid.Parse(id), cancellation);

            if (meta is null)
                return false;

            var deleteFileResult = await _fileHandler.DeleteFile($"{meta.Id}{meta.Extension}", cancellation);

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

    public async Task<DownloadFileDTO> GetFile(string id, CancellationToken cancellation = default)
    {
        try
        {
            var meta = await _metaService.GetById(Guid.Parse(id), cancellation);
            if (meta is null)
                return await Task.FromResult<DownloadFileDTO>(null!);

            var file = await _fileHandler.GetFile($"{meta.Id}{meta.Extension}", cancellation);

            return new DownloadFileDTO
            {
                Stream = file,
                FileName = meta.FileName + meta.Extension
            };
        }
        catch
        {
            return await Task.FromResult<DownloadFileDTO>(null!);
        }
    }

    public async Task<List<FileInfoDTO>> GetAllFiles(CancellationToken cancellation = default)
    {
        var getResult = await _metaService.GetAll(cancellation);
        return getResult
            .Select(x => new FileInfoDTO
                {
                    Id = x.Id,
                    Name = x.FileName,
                    Extension = x.Extension,
                    Size = x.Size})
            .ToList();
    }
}