using HOR0552.ViewModels;

namespace HOR0552.Views
{
    public partial class AddEventPage : ContentPage
    {
        private readonly AddEventViewModel _viewModel;
        public AddEventPage(AddEventViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            _viewModel = (AddEventViewModel)BindingContext;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnPageAppearing();
        }
    }
}

