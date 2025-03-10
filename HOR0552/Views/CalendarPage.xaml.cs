using HOR0552.ViewModels;

namespace HOR0552.Views
{
    public partial class CalendarPage : ContentPage
    {
        private CalendarViewModel _viewModel;
        public CalendarPage(CalendarViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            _viewModel = (CalendarViewModel)BindingContext;
        }
    }
}
