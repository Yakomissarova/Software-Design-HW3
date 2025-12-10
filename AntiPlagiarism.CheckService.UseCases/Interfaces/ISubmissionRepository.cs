using AntiPlagiarism.CheckService.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AntiPlagiarism.CheckService.UseCases.Interfaces;

public interface ISubmissionRepository
{
    Task AddAsync(Submission submission, CancellationToken ct = default);

    Task<Submission?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<IReadOnlyList<Submission>> GetByAssignmentAsync(
        string assignmentId,
        CancellationToken ct = default);
}