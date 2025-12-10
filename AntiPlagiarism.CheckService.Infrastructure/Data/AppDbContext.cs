using AntiPlagiarism.CheckService.Infrastructure.Data.Dto;
using Microsoft.EntityFrameworkCore;

namespace AntiPlagiarism.CheckService.Infrastructure.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<SubmissionDto> Submissions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SubmissionDto>(builder =>
        {
            builder.HasKey(s => s.Id);
            builder.ToTable("Submissions");

            builder.Property(s => s.StudentId)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(s => s.AssignmentId)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(s => s.FileId)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(s => s.SubmittedAt)
                .IsRequired();
        });
    }
}