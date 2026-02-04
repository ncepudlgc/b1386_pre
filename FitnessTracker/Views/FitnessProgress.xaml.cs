// FitnessTracker/Views/FitnessProgress.xaml.cs
using System.Windows;
using System.Windows.Controls;
using FitnessTracker.ViewModels;

namespace FitnessTracker.Views;

/// <summary>Code-behind is **UI-only**: shows dialogs and closes the window.</summary>
public partial class FitnessProgress : UserControl
{
    private FitnessProgressViewModel ViewModel => (FitnessProgressViewModel)DataContext;

    public FitnessProgress(FitnessProgressViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private async void OkButton_Click(object sender, RoutedEventArgs e)
    {
        if (!await ViewModel.SaveAsync())
        {
            MessageBox.Show(
                ViewModel.ValidationError ?? "Please enter a valid progress value.",
                "Invalid Input",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        var p = ViewModel.LastSavedProgress!;
        MessageBox.Show(
            $"{p.Type} progress saved: {p.Value} {p.Unit}",
            "Progress Saved",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

        CloseDialog();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) => CloseDialog();

    private void CloseDialog() => Window.GetWindow(this)?.Close();
}
