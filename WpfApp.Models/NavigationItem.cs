using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfApp.Models;
public class NavigationItem
{
    public required NavigationPage Page { get; set; }
    public required string Description { get; set; }
    public required Func<ObservableObject> GetViewModel { get; set; }
}

public class NavigationItems
{
    public List<NavigationItem> Items { get; set; } = new();
    public void Add(NavigationItem item) => Items.Add(item);
    public NavigationItem? Get(NavigationPage page) => Items.FirstOrDefault(s => s.Page == page);
}
