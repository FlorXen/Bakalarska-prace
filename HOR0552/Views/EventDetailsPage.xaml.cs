using HOR0552.ViewModels;

namespace HOR0552.Views
{
    public partial class EventDetailsPage : ContentPage
    {
        private readonly EventDetailsViewModel _viewModel;
        public EventDetailsPage(EventDetailsViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            _viewModel = (EventDetailsViewModel)BindingContext;
        }
    }
}