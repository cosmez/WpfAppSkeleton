using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfApp.Library.Models;
using WpfApp.Library.Services;

namespace WpfApp.Models.ViewModels;
public partial class GridPageViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService = null!;


    [ObservableProperty]
    private string? _text;

    public ObservableCollection<Employee> Employees { get; set; } = new();

    public GridPageViewModel()
    {

    }

    public GridPageViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }


    [RelayCommand]
    private async Task LoadData()
    {
        // Skip if the loading has already started
        var employees = await _databaseService.GetEmployees();
        foreach (var employee in employees) Employees.Add(employee);
    }
     

}