using Microsoft.EntityFrameworkCore;

namespace AntiPlagiarism.StorageService.Infrastructure.Data;

public class StorageDbContext : DbContext
{
    public StorageDbContext(DbContextOptions<StorageDbContext> options)
        : base(options)
    {
    }

    public DbSet<StoredFileDto> Files => Set<StoredFileDto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<StoredFileDto>(builder =>
        {
            builder.ToTable("files");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FileName)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.Path)
                .IsRequired()
                .HasMaxLength(1024);

            builder.Property(x => x.Size)
                .IsRequired();

            builder.Property(x => x.UploadedAt)
                .IsRequired();
        });
    }
}