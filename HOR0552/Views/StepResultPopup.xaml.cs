using CommunityToolkit.Maui.Views;
using HOR0552.ViewModels;

namespace HOR0552.Views;

public partial class StepResultPopup : Popup
{
    private readonly Action _onPopupClosed;
    public StepResultPopup(StepResultPopupViewModel vm, Action onPopupClosed)
    {
        InitializeComponent();
        BindingContext = vm;
        _onPopupClosed = onPopupClosed;
    }

    public void ClosePopup(object? sender, EventArgs e)
    {
        Close();
        _onPopupClosed?.Invoke();
    }
}
