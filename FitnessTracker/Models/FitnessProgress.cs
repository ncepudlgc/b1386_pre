// FitnessTracker/Models/FitnessProgress.cs
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FitnessTracker.Models;

/// <summary>
/// Represents a fitness progress entry (running distance or water intake).
/// </summary>
public class FitnessProgress
{
    public int Id { get; set; }

    [Required]
    public string Type { get; set; } = string.Empty;

    [Range(0.01, float.MaxValue, ErrorMessage = "Value must be greater than 0")]
    public float Value { get; set; }

    [Required]
    public string Unit { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore] 
    public bool IsRunningProgress => Type == "Running";
    
    [JsonIgnore] 
    public bool IsWaterProgress => Type == "Water";

    public RunningDistance ToRunningDistance()
    {
        if (!IsRunningProgress) 
            throw new InvalidOperationException("Progress is not a running progress");
        if (!Enum.TryParse<DistanceUnit>(Unit, out var unit))
            throw new InvalidOperationException($"Invalid distance unit: {Unit}");

        return new RunningDistance { Value = Value, Unit = unit };
    }

    public WaterContent ToWaterContent()
    {
        if (!IsWaterProgress) 
            throw new InvalidOperationException("Progress is not a water progress");
        if (!Enum.TryParse<WaterUnit>(Unit, out var unit))
            throw new InvalidOperationException($"Invalid water unit: {Unit}");

        return new WaterContent { Value = Value, Unit = unit };
    }

    public static FitnessProgress FromRunningDistance(RunningDistance running)
    {
        if (running == null) 
            throw new ArgumentNullException(nameof(running));
        if (running.Value <= 0) 
            throw new ArgumentException("Running distance value must be greater than 0", nameof(running));

        return new FitnessProgress
        {
            Type = "Running",
            Value = running.Value,
            Unit = running.Unit.ToString()
        };
    }

    public static FitnessProgress FromWaterContent(WaterContent water)
    {
        if (water == null) 
            throw new ArgumentNullException(nameof(water));
        if (water.Value <= 0) 
            throw new ArgumentException("Water content value must be greater than 0", nameof(water));

        return new FitnessProgress
        {
            Type = "Water",
            Value = water.Value,
            Unit = water.Unit.ToString()
        };
    }

    public FitnessProgress Clone() => new()
    {
        Id = Id,
        Type = Type,
        Value = Value,
        Unit = Unit,
        CreatedAt = CreatedAt
    };
}
