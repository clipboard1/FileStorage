namespace FileStorage.Application.Contracts;

public class SaveMetadataDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime UploadDate { get; set; }
}