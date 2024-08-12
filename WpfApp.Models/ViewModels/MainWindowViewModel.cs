using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Mime;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;
using WpfApp.Library.Models;
using WpfApp.Library.Services;
using CommunityToolkit.Mvvm.Messaging;
using WpfApp.Models.Messages;

namespace WpfApp.Models.ViewModels;
public partial class MainWindowViewModel : ObservableObject
{
    private readonly SampleService _sampleService = null!;
    private readonly DatabaseService _databaseService = null!;
    private readonly GridPageViewModel _gridPageViewModel = null!;
    private readonly NavigationItems _navigationItems = null!;

    [ObservableProperty]
    private string? _selectedPage;
    [ObservableProperty]
    private NavigationItem? _navigationItem;

    [ObservableProperty]
    private ObservableObject? _currentPageViewModel;
    
    //public ObservableObject CurrentPageViewModel { get; set; } = null!;
    public ObservableCollection<NavigationItem> NavigationItems { get; set; } = new();
    

    public MainWindowViewModel()
    { 
    }

    public MainWindowViewModel(SampleService sampleService, DatabaseService databaseService,  NavigationItems navigationItems)
    {
        _sampleService = sampleService;
        _databaseService = databaseService; 
        _navigationItems = navigationItems;
        CurrentPageViewModel = navigationItems.Get(NavigationPage.Grid)?.GetViewModel();

        foreach (var item in navigationItems.Items)
        {
            NavigationItems.Add(item);
        }

        //messengers
        // Register a message in some module
        WeakReferenceMessenger.Default.Register<NavigationPageChangedMessage>(this, NavigationChanged);
        //LoadDocsCommand = new AsyncRelayCommand<string>(LoadDocsAsync);
    }

    private void NavigationChanged(object recipient, NavigationPageChangedMessage message)
    {
        Debug.WriteLine($"{message.Value}");
        var selectedPage = _navigationItems.Get(message.Value);
        if (selectedPage is not null)
        {
            CurrentPageViewModel = selectedPage.GetViewModel();
        }
    }

    [RelayCommand]
    private void SelectItem()
    {
        if (_navigationItem is not null)
        {
            WeakReferenceMessenger.Default.Send(new NavigationPageChangedMessage(_navigationItem.Page));
        }
    }
 

     

}
