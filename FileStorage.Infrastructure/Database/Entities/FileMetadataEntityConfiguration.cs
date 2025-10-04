using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileStorage.Infrastructure.Database.Entities;

public class FileMetadataEntityConfiguration : IEntityTypeConfiguration<FileMetadataEntity>
{
    public void Configure(EntityTypeBuilder<FileMetadataEntity> builder)
    {
        builder
            .HasKey(f => f.Id);

        builder
            .Property(f => f.FileName)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(f => f.FileName)
            .HasMaxLength(10)
            .IsRequired();

        builder
            .Property(f => f.Size)
            .IsRequired();

        builder
            .Property(f => f.UploadDate)
            .IsRequired();
    }
}