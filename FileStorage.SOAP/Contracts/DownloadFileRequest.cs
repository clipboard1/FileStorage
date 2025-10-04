using CoreWCF;

namespace FileStorage.SOAP.Contracts;

[MessageContract]
public class DownloadFileRequest
{
    [MessageHeader]
    public string FileId { get; set; } = default!;
}