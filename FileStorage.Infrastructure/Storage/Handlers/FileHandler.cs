using FileStorage.Infrastructure.Storage.Abstractions;

namespace FileStorage.Infrastructure.Storage.Handlers;

public class FileHandler : IFileHandler
{
    public readonly string StoragePath = OperatingSystem.IsWindows() ? @"C:\Temp\Files" : "/app/data/files";


    public FileHandler()
    {
        if (!Directory.Exists(StoragePath))
            Directory.CreateDirectory(StoragePath);
    }

    public async Task<bool> SaveFile(FileStream file, string filename, CancellationToken cancellation = default)
    {
        if (file is null)
            throw new ArgumentException("Empty file");

        if (string.IsNullOrEmpty(filename))
            throw new ArgumentException("Empty filename");

        try
        {
            var fullPath = Path.Combine(StoragePath, filename);

            if (file.CanSeek)
                file.Position = 0;

            await using var destination = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(destination, cancellation);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public Task<bool> DeleteFile(string filename, CancellationToken cancellation = default)
    {
        try
        {
            var fullPath = Path.Combine(StoragePath, filename);

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public Task<FileStream> GetFile(string filename, CancellationToken cancellation = default)
    {
        var fullPath = Path.Combine(StoragePath, filename);

        if (!File.Exists(fullPath))
            return Task.FromResult<FileStream>(null!);

        var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Task.FromResult(stream);
    }
}