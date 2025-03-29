using HOR0552.ViewModels;

namespace HOR0552.Views
{
    public partial class DiagnosisDetailCalendarPage : ContentPage
    {
        private readonly DiagnosisDetailCalendarViewModel _viewModel;
        public DiagnosisDetailCalendarPage(DiagnosisDetailCalendarViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            _viewModel = (DiagnosisDetailCalendarViewModel)BindingContext;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnPageAppearing();
        }
    }
}
