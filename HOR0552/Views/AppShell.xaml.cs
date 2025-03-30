using HOR0552.Views;

namespace HOR0552
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(DiagnosisDetailPage), typeof(DiagnosisDetailPage));

            Routing.RegisterRoute(nameof(SelectDiagnosisPage), typeof(SelectDiagnosisPage));

            Routing.RegisterRoute(nameof(AddEventPage), typeof(AddEventPage));

            Routing.RegisterRoute(nameof(EventDetailsPage), typeof(EventDetailsPage));

            Routing.RegisterRoute(nameof(EditEventPage), typeof(EditEventPage));

            Routing.RegisterRoute(nameof(DiagnosisDetailCalendarPage), typeof(DiagnosisDetailCalendarPage));

            Routing.RegisterRoute(nameof(DiagnosisStepsPage), typeof(DiagnosisStepsPage));
        }

        
    }
}
