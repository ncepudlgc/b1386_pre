// FitnessTracker/ViewModels/FitnessProgressViewModel.cs
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FitnessTracker.Models;
using FitnessTracker.Services;
using Microsoft.Extensions.Logging;

namespace FitnessTracker.ViewModels;

/// <summary>
/// View-model for the Fitness Progress entry dialog.
/// Contains all validation and persistence logic, injected with <see cref="IFitnessProgressService"/>.
/// </summary>
public class FitnessProgressViewModel : INotifyPropertyChanged
{
    private readonly IFitnessProgressService _progressService;
    private readonly ILogger<FitnessProgressViewModel>? _logger;

    public FitnessProgressViewModel(
        IFitnessProgressService progressService, 
        ILogger<FitnessProgressViewModel>? logger = null)
    {
        _progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
        _logger = logger;
    }

    private string _selectedProgressType = "Running";
    private float _progressValue;
    private DistanceUnit _selectedDistanceUnit = DistanceUnit.Miles;
    private WaterUnit _selectedWaterUnit = WaterUnit.Ounces;
    private FitnessProgress? _lastSavedProgress;
    private string? _validationError;
    private bool _isSaving;

    public string[] ProgressTypes { get; } = { "Running", "Water" };
    public DistanceUnit[] DistanceUnits { get; } = Enum.GetValues<DistanceUnit>();
    public WaterUnit[] WaterUnits { get; } = Enum.GetValues<WaterUnit>();

    public string SelectedProgressType
    {
        get => _selectedProgressType;
        set => SetField(ref _selectedProgressType, value);
    }

    public float ProgressValue
    {
        get => _progressValue;
        set
        {
            if (SetField(ref _progressValue, value))
            {
                ValidateProgressValue();
            }
        }
    }

    public DistanceUnit SelectedDistanceUnit
    {
        get => _selectedDistanceUnit;
        set
        {
            if (_selectedDistanceUnit != value)
            {
                var previous = _selectedDistanceUnit;
                _selectedDistanceUnit = value;

                if (IsRunningProgress && ProgressValue > 0)
                {
                    try
                    {
                        ProgressValue = new RunningDistance
                        {
                            Unit = previous,
                            Value = ProgressValue
                        }.ConvertTo(value);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "Failed to convert distance units from {Previous} to {New}", previous, value);
                    }
                }

                OnPropertyChanged();
            }
        }
    }

    public WaterUnit SelectedWaterUnit
    {
        get => _selectedWaterUnit;
        set
        {
            if (_selectedWaterUnit != value)
            {
                var previous = _selectedWaterUnit;
                _selectedWaterUnit = value;

                if (IsWaterProgress && ProgressValue > 0)
                {
                    try
                    {
                        ProgressValue = new WaterContent
                        {
                            Unit = previous,
                            Value = ProgressValue
                        }.ConvertTo(value);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "Failed to convert water units from {Previous} to {New}", previous, value);
                    }
                }

                OnPropertyChanged();
            }
        }
    }

    public bool IsRunningProgress => SelectedProgressType == "Running";
    public bool IsWaterProgress => SelectedProgressType == "Water";

    public FitnessProgress? LastSavedProgress
    {
        get => _lastSavedProgress;
        private set => SetField(ref _lastSavedProgress, value);
    }

    public string? ValidationError
    {
        get => _validationError;
        private set => SetField(ref _validationError, value);
    }

    public bool IsSaving
    {
        get => _isSaving;
        private set => SetField(ref _isSaving, value);
    }

    public bool IsValid => string.IsNullOrEmpty(ValidationError);

    /// <summary>
    /// Validates input and saves the progress entry.
    /// Returns <c>true</c> on success; <c>false</c> means validation failed.
    /// </summary>
    public async Task<bool> SaveAsync()
    {
        if (IsSaving) return false;

        ValidateProgressValue();
        if (!IsValid) return false;

        IsSaving = true;
        try
        {
            FitnessProgress saved;
            if (IsRunningProgress)
            {
                saved = await _progressService.SaveProgressAsync(
                    FitnessProgress.FromRunningDistance(GetRunningProgress()));
            }
            else if (IsWaterProgress)
            {
                saved = await _progressService.SaveProgressAsync(
                    FitnessProgress.FromWaterContent(GetWaterProgress()));
            }
            else
            {
                ValidationError = "Invalid progress type selected";
                return false;
            }

            LastSavedProgress = saved;
            ValidationError = null;
            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to save progress of type {Type} with value {Value}", SelectedProgressType, ProgressValue);
            ValidationError = "Failed to save progress. Please try again.";
            return false;
        }
        finally
        {
            IsSaving = false;
        }
    }

    private void ValidateProgressValue()
    {
        if (ProgressValue <= 0)
        {
            ValidationError = "Progress value must be greater than 0";
        }
        else if (float.IsNaN(ProgressValue) || float.IsInfinity(ProgressValue))
        {
            ValidationError = "Progress value must be a valid number";
        }
        else
        {
            ValidationError = null;
        }
    }

    public RunningDistance GetRunningProgress() => new()
    {
        Unit = SelectedDistanceUnit,
        Value = ProgressValue
    };

    public WaterContent GetWaterProgress() => new()
    {
        Unit = SelectedWaterUnit,
        Value = ProgressValue
    };

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        if (name == nameof(SelectedProgressType))
        {
            OnPropertyChanged(nameof(IsRunningProgress));
            OnPropertyChanged(nameof(IsWaterProgress));
            ValidateProgressValue();
        }
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(name);
        return true;
    }
}
