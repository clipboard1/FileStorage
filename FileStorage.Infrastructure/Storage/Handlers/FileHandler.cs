using FileStorage.Infrastructure.Storage.Abstractions;

namespace FileStorage.Infrastructure.Storage.Handlers;

public class FileHandler : IFileHandler
{
    public readonly string StoragePath = OperatingSystem.IsWindows() ? @"C:\Temp\Files" : "/app/data/files";


    public FileHandler(string storagePath = "")
    {
        if (!string.IsNullOrEmpty(storagePath))
            StoragePath = storagePath;

        if (!Directory.Exists(StoragePath))
            Directory.CreateDirectory(StoragePath);
    }

    public async Task<Result<Guid>> SaveFile(Stream file, CancellationToken cancellation = default)
    {
        if (file is null)
            throw new ArgumentException("Empty file");

        try
        {
            var guid  = Guid.NewGuid();
            var fullPath = Path.Combine(StoragePath, guid.ToString());

            if (file.CanSeek)
                file.Position = 0;

            await using var destination = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(destination, cancellation);

            return Result<Guid>.Success(guid);
        }
        catch  (Exception ex)
        {
            return Result<Guid>.Failure(Result.ToDict("General", ex.Message));
        }
    }

    public async Task<Result> DeleteFile(string id, CancellationToken cancellation = default)
    {
        try
        {
            var fullPath = Path.Combine(StoragePath, id);

            if (!File.Exists(fullPath))
                return Result<FileStream>.Failure(Result.ToDict("File", "File not found"));

            File.Delete(fullPath);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Result.ToDict("General", ex.Message));
        }
    }

    public async Task<Result<FileStream>> GetFile(string id, CancellationToken cancellation = default)
    {
        var fullPath = Path.Combine(StoragePath, id);

        if (!File.Exists(fullPath))
            return Result<FileStream>.Failure(Result.ToDict("File", "File not found"));

        var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Result<FileStream>.Success(stream);
    }
}