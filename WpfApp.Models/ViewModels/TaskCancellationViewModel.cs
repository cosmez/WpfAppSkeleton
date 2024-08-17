using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading;
using WpfApp.Library.Models;
using WpfApp.Library.Services;

namespace WpfApp.Models.ViewModels;
public partial class TaskCancellationViewModel : ObservableObject
{
    private CancellationTokenSource? _cancellationToken;
    
    [ObservableProperty]
    private string _progressText;

    [ObservableProperty] private bool _isRunning = false;


    public TaskCancellationViewModel()
    {
        ProgressText = "Click To Run Task";
    }

    [RelayCommand]
    private async Task RunLongTask()
    {
        IsRunning = true;
        _cancellationToken = new CancellationTokenSource();
        ProgressText = "Running Task";
        try
        {
            //notify the UI that the task can be cancelled with another button
            StopLongTaskCommand.NotifyCanExecuteChanged();
            //run a task for 5 minutes
            await Task.Delay(TimeSpan.FromMinutes(5), cancellationToken: _cancellationToken.Token);
        }
        catch (TaskCanceledException ex)
        {
            ProgressText = ex.Message;
        }

        IsRunning = false;
    }

    //Helper for Stop Command
    private bool CanStopTask() => IsRunning;
    
    [RelayCommand(CanExecute = nameof(CanStopTask))]
    private async Task StopLongTask()
    {
        //send cancellation message through CancellationTokenSource
        if (_cancellationToken is not null)
            await _cancellationToken.CancelAsync();
        
    }
}
