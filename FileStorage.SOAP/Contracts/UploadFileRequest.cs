using CoreWCF;

namespace FileStorage.SOAP.Contracts;

[MessageContract]
public class UploadFileRequest
{
    [MessageHeader]
    public string FileName { get; set; }

    [MessageBodyMember]
    public byte[] FileContent { get; set; }
}
