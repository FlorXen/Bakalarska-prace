using HOR0552.ViewModels;

namespace HOR0552.Views
{
    public partial class CalendarPage : ContentPage
    {
        private readonly CalendarViewModel _viewModel;
        public CalendarPage(CalendarViewModel vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm), "ViewModel cannot be null");

            InitializeComponent();
            Console.WriteLine("InitializeComponent completed");

            BindingContext = vm;
            Console.WriteLine("BindingContext set");

            _viewModel = (CalendarViewModel)BindingContext;
            Console.WriteLine("ViewModel set");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnPageAppearing();
        }
    }
}
