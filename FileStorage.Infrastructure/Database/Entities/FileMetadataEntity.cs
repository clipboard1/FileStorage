namespace FileStorage.Infrastructure.Database.Entities;

public class FileMetadataEntity
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime UploadDate { get; set; }
}