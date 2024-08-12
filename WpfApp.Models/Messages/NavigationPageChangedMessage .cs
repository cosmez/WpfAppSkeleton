using CommunityToolkit.Mvvm.Messaging.Messages;

namespace WpfApp.Models.Messages;
public class NavigationPageChangedMessage : ValueChangedMessage<NavigationPage>
{
    public NavigationPageChangedMessage(NavigationPage value) : base(value)
    {

    }
}
