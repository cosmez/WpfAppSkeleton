namespace WpfApp.Models;

/// <summary>
/// This is the basic building block for a dialog page. this covers the most basic dialogs of every WPf application.
/// TODO: implement an example of a Task dialog that uses an ObservableObject to render his content
/// </summary>
public interface IDialogService
{
    bool ShowConfirmationDialog(string message, string title);
    
    void ShowMessageDialog(string message, string title);
    void ShowMessageInformationDialog(string message, string title);
    void ShowMessageErrorDialog(string message, string title);
    string[]? ShowOpenFileDialog(string message, string title, string filter = "All files (*.*)|*.*", bool multiSelect = false);
    string[]? ShowSaveFileDialog(string message, string title, string filter = "All files (*.*)|*.*");
    string[]? ShowFolderDialog(string message, string title, bool multiSelect = false);
}