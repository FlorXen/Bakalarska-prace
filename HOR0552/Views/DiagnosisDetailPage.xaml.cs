using HOR0552.ViewModels;

namespace HOR0552.Views;

public partial class DiagnosisDetailPage : ContentPage
{
	public DiagnosisDetailPage(DiagnosisDetailViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}