// FitnessTracker/Repositories/IFitnessProgressRepository.cs
using FitnessTracker.Models;

namespace FitnessTracker.Repositories;

/// <summary>
/// Responsible for on-disk persistence of fitness progress entries.
/// </summary>
public interface IFitnessProgressRepository : IDisposable
{
    Task LoadAsync();
    Task<FitnessProgress> AddProgressAsync(FitnessProgress progress);
    Task<List<FitnessProgress>> GetAllProgressAsync();
    Task<List<FitnessProgress>> GetProgressByTypeAsync(string type);
}
