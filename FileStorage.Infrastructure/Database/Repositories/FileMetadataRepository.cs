using FileStorage.Infrastructure.Database.Abstractions;
using FileStorage.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileStorage.Infrastructure.Database.Repositories;

public class FileMetadataRepository : IFileMetadataRepository
{
    private readonly ApplicationDbContext _context;

    public FileMetadataRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FileMetadataEntity> SaveMetadata(FileMetadataEntity metadata, CancellationToken cancellation = default)
    {
        if (metadata.Id == Guid.Empty)
            throw new ArgumentNullException(nameof(metadata.Id), "Id cannot be empty");

        var existingMetaData = await _context.FilesMetadata
            .AnyAsync(f => f.Id == metadata.Id, cancellation);

        if (existingMetaData)
            throw new InvalidOperationException("This file already saved");

        await _context.FilesMetadata.AddAsync(metadata, cancellation);
        await _context.SaveChangesAsync(cancellation);

        return metadata;
    }

    public async Task DeleteMetadata(Guid id, CancellationToken cancellation = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentNullException(nameof(id), "Id cannot be empty");

        var deletedRows = await _context.FilesMetadata
            .Where(f => f.Id == id)
            .ExecuteDeleteAsync(cancellation);

        if (deletedRows == 0)
            throw new InvalidOperationException("File doesnt exists");
    }

    public async Task<List<FileMetadataEntity>> GetAll(CancellationToken cancellation = default)
    {
        var meta = await _context.FilesMetadata
            .ToListAsync(cancellation);

        return meta;
    }

    public async Task<FileMetadataEntity?> GetById(Guid id, CancellationToken cancellation = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentNullException(nameof(id), "Id cannot be empty");

        var meta = await _context.FilesMetadata
            .FirstOrDefaultAsync(f => f.Id == id, cancellation);

        return meta;
    }
}