// FitnessTracker/Services/FitnessProgressService.cs
using FitnessTracker.Models;
using FitnessTracker.Repositories;

namespace FitnessTracker.Services;

public sealed class FitnessProgressService : IFitnessProgressService, IDisposable
{
    private readonly IFitnessProgressRepository _repo;

    public FitnessProgressService(IFitnessProgressRepository repo)
        => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

    public Task LoadProgressAsync() => _repo.LoadAsync();

    public async Task<FitnessProgress> SaveProgressAsync(FitnessProgress progress)
    {
        if (progress == null) 
            throw new ArgumentNullException(nameof(progress));

        progress.CreatedAt = DateTime.UtcNow;
        return await _repo.AddProgressAsync(progress);
    }

    public Task<List<FitnessProgress>> GetAllProgressAsync() 
        => _repo.GetAllProgressAsync();

    public Task<List<FitnessProgress>> GetProgressByTypeAsync(string type) 
        => _repo.GetProgressByTypeAsync(type);

    public void Dispose() => (_repo as IDisposable)?.Dispose();
}
