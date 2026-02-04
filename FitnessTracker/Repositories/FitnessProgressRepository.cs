// FitnessTracker/Repositories/FitnessProgressRepository.cs
using System.IO;
using System.Text.Json;
using FitnessTracker.Models;
using Microsoft.Extensions.Logging;

namespace FitnessTracker.Repositories;

public sealed class FitnessProgressRepository : IFitnessProgressRepository
{
    private readonly List<FitnessProgress> _progressEntries = new();
    private readonly SemaphoreSlim _sem = new(1, 1);
    private readonly ILogger<FitnessProgressRepository>? _log;

    private readonly string _dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SaveData");
    private readonly string _file;

    private int _nextId = 1;

    public FitnessProgressRepository(ILogger<FitnessProgressRepository>? log = null)
    {
        _log = log;
        _file = Path.Combine(_dir, "fitness_progress.json");
        Directory.CreateDirectory(_dir);
    }

    public async Task LoadAsync()
    {
        await _sem.WaitAsync();
        try
        {
            if (!File.Exists(_file)) return;

            await using var s = File.OpenRead(_file);
            var loaded = await JsonSerializer.DeserializeAsync<List<FitnessProgress>>(s);
            if (loaded is null) return;

            _progressEntries.Clear();
            _progressEntries.AddRange(loaded.Select(p => p.Clone()));
            _nextId = _progressEntries.Any() ? _progressEntries.Max(p => p.Id) + 1 : 1;
        }
        catch (Exception ex)
        {
            _log?.LogError(ex, "Failed to load fitness progress from {File}", _file);
        }
        finally 
        { 
            _sem.Release(); 
        }
    }

    public async Task<FitnessProgress> AddProgressAsync(FitnessProgress progress)
    {
        if (progress == null) 
            throw new ArgumentNullException(nameof(progress));

        await _sem.WaitAsync();
        try
        {
            var toStore = progress.Clone();
            toStore.Id = _nextId++;
            if (toStore.CreatedAt == default)
                toStore.CreatedAt = DateTime.UtcNow;

            _progressEntries.Add(toStore);
            await PersistAsync();
            return toStore.Clone();
        }
        finally 
        { 
            _sem.Release(); 
        }
    }

    public async Task<List<FitnessProgress>> GetAllProgressAsync()
    {
        await _sem.WaitAsync();
        try 
        { 
            return _progressEntries.Select(p => p.Clone()).ToList(); 
        }
        finally 
        { 
            _sem.Release(); 
        }
    }

    public async Task<List<FitnessProgress>> GetProgressByTypeAsync(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            return new List<FitnessProgress>();

        await _sem.WaitAsync();
        try
        {
            return _progressEntries
                .Where(p => p.Type == type)
                .Select(p => p.Clone())
                .ToList();
        }
        finally 
        { 
            _sem.Release(); 
        }
    }

    private async Task PersistAsync()
    {
        var json = JsonSerializer.Serialize(_progressEntries, new JsonSerializerOptions 
        { 
            WriteIndented = true 
        });
        var tmp = Path.Combine(_dir, $"fp_{Guid.NewGuid():N}.tmp");

        try
        {
            await File.WriteAllTextAsync(tmp, json);
            File.Move(tmp, _file, overwrite: true);
        }
        catch (Exception ex)
        {
            _log?.LogError(ex, "Failed to persist fitness progress to {File}", _file);
            throw;
        }
        finally
        {
            if (File.Exists(tmp)) 
                File.Delete(tmp);
        }
    }

    public void Dispose() => _sem.Dispose();
}
