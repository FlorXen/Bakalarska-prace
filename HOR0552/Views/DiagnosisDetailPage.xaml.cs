using HOR0552.ViewModels;

namespace HOR0552.Views;

public partial class DiagnosisDetailPage : ContentPage
{
    private readonly DiagnosisDetailViewModel _viewModel;
    public DiagnosisDetailPage(DiagnosisDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _viewModel = (DiagnosisDetailViewModel)BindingContext;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnPageAppearing();
    }
}