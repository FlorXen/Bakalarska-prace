using HOR0552.ViewModels;

namespace HOR0552.Views
{
    public partial class EditEventPage : ContentPage
    {
        private readonly EditEventViewModel _viewModel;
        public EditEventPage(EditEventViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            _viewModel = (EditEventViewModel)BindingContext;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnPageAppearing();
        }
    }
}

