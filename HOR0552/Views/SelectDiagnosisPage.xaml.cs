using HOR0552.ViewModels;

namespace HOR0552.Views
{
    public partial class SelectDiagnosisPage : ContentPage
    {
        public SelectDiagnosisPage(SelectDiagnosisViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = BindingContext as SelectDiagnosisViewModel;
            viewModel?.FilterDiagnoses(e.NewTextValue);
        }
    }
}
