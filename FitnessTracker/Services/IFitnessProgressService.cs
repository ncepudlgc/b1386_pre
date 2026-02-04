// FitnessTracker/Services/IFitnessProgressService.cs
using FitnessTracker.Models;

namespace FitnessTracker.Services;

/// <summary>
/// Orchestrates fitness progress business logic.
/// </summary>
public interface IFitnessProgressService
{
    Task LoadProgressAsync();
    Task<FitnessProgress> SaveProgressAsync(FitnessProgress progress);
    Task<List<FitnessProgress>> GetAllProgressAsync();
    Task<List<FitnessProgress>> GetProgressByTypeAsync(string type);
}
