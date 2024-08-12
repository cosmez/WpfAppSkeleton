using System.Collections.ObjectModel;
using System.Net.Mime;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;
using WpfApp.Library.Models;
using WpfApp.Library.Services;

namespace WpfApp.Models.ViewModels;
public partial class MainWindowViewModel : ObservableObject
{
    private readonly SampleService _sampleService = null!;
    private readonly DatabaseService _databaseService = null!;
    

    [ObservableProperty]
    private string? _text;

    public ObservableCollection<Employee> Employees { get; set; } = new();

    public MainWindowViewModel()
    {
        
    }

    public MainWindowViewModel(SampleService sampleService, DatabaseService databaseService)
    {
        _sampleService = sampleService;
        _databaseService = databaseService;


        //LoadDocsCommand = new AsyncRelayCommand<string>(LoadDocsAsync);
    }


    [RelayCommand]
    private async Task LoadData()
    {
        // Skip if the loading has already started
        var employees = await _databaseService.GetEmployees();
        foreach (var employee in employees) Employees.Add(employee);
    }

    [RelayCommand]
    private void SayHello()
    {
        // Skip if the loading has already started
        Text = _sampleService.GetString();
    }

}
