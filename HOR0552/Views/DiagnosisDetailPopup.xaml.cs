using CommunityToolkit.Maui.Views;
using HOR0552.ViewModels;

namespace HOR0552.Views;

public partial class DiagnosisDetailPopup : Popup
{
    public DiagnosisDetailPopup(DiagnosisDetailPopupViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    void ClosePopup(object? sender, EventArgs e) => Close();
}
