namespace FileStorage.Application.Contracts;

public class DownloadFileDTO
{
    public FileStream Stream { get; set; }
    public string FileName { get; set; }
}