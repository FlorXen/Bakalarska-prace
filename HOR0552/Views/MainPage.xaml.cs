using HOR0552.ViewModels;

namespace HOR0552.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;
        public MainPage(MainPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            _viewModel = (MainPageViewModel)BindingContext;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnPageAppearing();
        }
    }
}
