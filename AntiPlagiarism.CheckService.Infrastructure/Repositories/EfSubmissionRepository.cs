using AntiPlagiarism.CheckService.Entities;
using AntiPlagiarism.CheckService.Infrastructure.Data;
using AntiPlagiarism.CheckService.Infrastructure.Data.Dto;
using AntiPlagiarism.CheckService.UseCases.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AntiPlagiarism.CheckService.Infrastructure.Repositories;

internal sealed class EfSubmissionRepository : ISubmissionRepository
{
    private readonly AppDbContext _db;

    public EfSubmissionRepository(AppDbContext db)
    {
        _db = db;
    }

    private static Submission MapToEntity(SubmissionDto dto) =>
        new(dto.StudentId, dto.AssignmentId, dto.FileId)
        {
            Id = dto.Id,
            SubmittedAt = dto.SubmittedAt
        };

    private static SubmissionDto MapToDto(Submission entity) =>
        new(entity.Id, entity.StudentId, entity.AssignmentId, entity.FileId, entity.SubmittedAt);

    public async Task AddAsync(Submission submission, CancellationToken ct = default)
    {
        var dto = MapToDto(submission);
        await _db.Submissions.AddAsync(dto, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<Submission?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Submissions.FindAsync(new object[] { id }, ct);
        return dto is null ? null : MapToEntity(dto);
    }

    public async Task<IReadOnlyList<Submission>> GetByAssignmentAsync(
        string assignmentId,
        CancellationToken ct = default)
    {
        var dtos = await _db.Submissions
            .Where(s => s.AssignmentId == assignmentId)
            .ToListAsync(ct);

        return dtos
            .Select(MapToEntity)
            .ToList();
    }
}