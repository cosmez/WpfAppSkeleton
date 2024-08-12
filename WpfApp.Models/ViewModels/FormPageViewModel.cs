using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using WpfApp.Library.Models;
using WpfApp.Library.Services;

namespace WpfApp.Models.ViewModels;
public partial class FormPageViewModel : ObservableObject
{
    private readonly SampleService _sampleService = null!;


    public ObservableCollection<Employee> Employees { get; set; } = new();

    public FormPageViewModel()
    {

    }

    public FormPageViewModel(SampleService sampleService)
    {
        _sampleService = sampleService;
    }

}