using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfApp.Library.Services;

namespace WpfApp.Models.ViewModels;
public partial class PdfPageViewModel : ObservableObject
{
    private readonly IDialogService _dialgoService = null!;

    [ObservableProperty]
    private string? _filename;
    
    public PdfPageViewModel() {}

    public PdfPageViewModel(IDialogService dialgoService)
    {
        _dialgoService = dialgoService;
    }


    [RelayCommand]
    public void LoadPdf()
    {
        var files = _dialgoService.ShowOpenFileDialog("Select PDF File", "Open PDF", "PDF Files (*.pdf)|*.pdf");
        if (files is not null && files.Length > 0)
        {
            Filename = files[0];
        }
    }

}