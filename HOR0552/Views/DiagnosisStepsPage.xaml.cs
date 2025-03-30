using HOR0552.ViewModels;

namespace HOR0552.Views;

public partial class DiagnosisStepsPage : ContentPage
{
    private readonly DiagnosisStepsViewModel _viewModel;
    public DiagnosisStepsPage(DiagnosisStepsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _viewModel = (DiagnosisStepsViewModel)BindingContext;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnPageAppearing();
    }
}