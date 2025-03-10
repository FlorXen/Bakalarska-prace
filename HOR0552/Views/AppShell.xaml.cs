using HOR0552.Views;

namespace HOR0552
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

            Routing.RegisterRoute(nameof(CalendarPage), typeof(CalendarPage));

            Routing.RegisterRoute(nameof(DiagnosisDetailPage), typeof(DiagnosisDetailPage));

            Routing.RegisterRoute(nameof(SelectDiagnosisPage), typeof(SelectDiagnosisPage));
        }
    }
}
