using FileStorage.Application.Abstractions;
using FileStorage.Application.Contracts;
using FileStorage.SOAP.Abstractions;
using FileStorage.SOAP.Contracts;

namespace FileStorage.SOAP.Services;

public class FileSoapService : IFileSoapService
{
    private readonly IFileService _fileService;

    public FileSoapService(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task<UploadFileResponse> SaveFile(UploadFileRequest request)
    {
        using var stream = new MemoryStream(request.FileContent);
        var id = await _fileService.SaveFile(stream, request.FileName, CancellationToken.None);
        return new UploadFileResponse { FileId = id };
    }

    public Task<bool> DeleteFile(string id)
    {
        return _fileService.DeleteFile(id, CancellationToken.None);
    }

    public async Task<DownloadFileResponse> GetFile(DownloadFileRequest request)
    {
        var fileDto = await _fileService.GetFile(request.FileId, CancellationToken.None);

        using var ms = new MemoryStream();
        await fileDto.Stream.CopyToAsync(ms);

        return new DownloadFileResponse
        {
            FileName = fileDto.FileName,
            FileContent = ms.ToArray()
        };
    }

    public Task<List<FileInfoDTO>> GetAllFiles()
    {
        return _fileService.GetAllFiles(CancellationToken.None);
    }
}