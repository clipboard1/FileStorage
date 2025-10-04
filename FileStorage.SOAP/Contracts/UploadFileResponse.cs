using CoreWCF;

namespace FileStorage.SOAP.Contracts;

[MessageContract]
public class UploadFileResponse
{
    [MessageBodyMember]
    public Guid FileId { get; set; }
}