namespace FileStorage.Application.Contracts;

public class FileInfoDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
    public long Size { get; set; }
}