using CoreWCF;
using FileStorage.Application.Contracts;
using FileStorage.SOAP.Contracts;

namespace FileStorage.SOAP.Abstractions;

[ServiceContract]
public interface IFileSoapService
{
    [OperationContract]
    Task<UploadFileResponse> SaveFile(UploadFileRequest request);

    [OperationContract]
    Task<bool> DeleteFile(string id);

    [OperationContract]
    Task<DownloadFileResponse> GetFile(DownloadFileRequest request);

    [OperationContract]
    Task<List<FileInfoDTO>> GetAllFiles();
}