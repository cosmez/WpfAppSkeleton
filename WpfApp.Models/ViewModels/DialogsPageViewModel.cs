using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfApp.Library.Models;
using WpfApp.Library.Services;

namespace WpfApp.Models.ViewModels;
public partial class DialogsPageViewModel : ObservableObject
{
    private readonly IDialogService _dialgoService = null!;
    private readonly SampleService _sampleService = null!;


    [ObservableProperty]
    private bool _confirmationResult;
    [ObservableProperty] 
    private string?  _selectedFolder;
    [ObservableProperty]
    private string? _saveFile;
    public ObservableCollection<string> SelectedFiles { get; set; } = new();

    public DialogsPageViewModel()
    {

    }
    
    

    public DialogsPageViewModel(SampleService sampleService, IDialogService dialgoService)
    {
        _sampleService = sampleService;
        _dialgoService = dialgoService;
    }


    [RelayCommand]
    private void ShowConfirmationDialog()
    {
        ConfirmationResult = _dialgoService.ShowConfirmationDialog("Confirmation Dialog", "Test Title");
    }


    [RelayCommand]
    private void ShowAllMessageDialogs()
    {
        _dialgoService.ShowMessageDialog("Message #1", "Test Title");
        _dialgoService.ShowMessageErrorDialog("Message Error", "Test Title");
        _dialgoService.ShowMessageInformationDialog("Message Information", "Test Title");
    }

    [RelayCommand]
    private void ShowFolderDialog()
    {
        var folders = _dialgoService.ShowFolderDialog("Select Folder", "title");
        if (folders is not null && folders.Any())
        {
            SelectedFolder = folders.First();
        }
    }

    [RelayCommand]
    private void ShowFileDialog()
    {
        var files = _dialgoService.ShowOpenFileDialog("Select Folder", "title", multiSelect:true, filter: "PDF Files (*.pdf)|*.pdf");
        if (files is not null && files.Any())
        {
            SelectedFiles.Clear();
            foreach (var file in files)
            {
                SelectedFiles.Add(file);
            }
        }
    }

    [RelayCommand]
    private void ShowSaveDialog()
    {
        var files = _dialgoService.ShowSaveFileDialog("Select Folder", "title");
        if (files is not null && files.Any())
        {
            SaveFile = files.First();
        }
    }
}