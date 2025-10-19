using FileStorage.Application.Abstractions;
using FileStorage.Infrastructure;
using FileStorage.Infrastructure.Storage.Abstractions;

namespace FileStorage.Application.Services;

public class FileService : IFileService
{
    private readonly IFileHandler _fileHandler;

    public FileService(IFileHandler fileHandler)
    {
        _fileHandler = fileHandler;
    }

    public async Task<Result<Guid>> SaveFile(Stream file, CancellationToken cancellation = default)
    {
        try
        {
            var result = await _fileHandler.SaveFile(file, cancellation);

            if (result.IsFailure)
                return Result<Guid>.Failure(result.Errors);

            return Result<Guid>.Success(result.Value);
        }
        catch  (Exception ex)
        {
            return Result<Guid>.Failure(Result.ToDict("General", ex.Message));
        }

    }

    public async Task<Result> DeleteFile(Guid id, CancellationToken cancellation = default)
    {
        try
        {
            var deleteResult = await _fileHandler.DeleteFile(id.ToString(), cancellation);

            if (deleteResult.IsFailure)
                return Result.Failure(deleteResult.Errors);

            return Result.Success();
        }
        catch  (Exception ex)
        {
            return Result<Guid>.Failure(Result.ToDict("General", ex.Message));
        }
    }

    public async Task<Result<FileStream>> GetFile(Guid id, CancellationToken cancellation = default)
    {
        try
        {
            var getResult = await _fileHandler.GetFile(id.ToString(), cancellation);

            if (getResult.IsFailure)
                return Result<FileStream>.Failure(getResult.Errors);

            return Result<FileStream>.Success(getResult.Value);
        }
        catch  (Exception ex)
        {
            return Result<FileStream>.Failure(Result.ToDict("General", ex.Message));
        }
    }
}