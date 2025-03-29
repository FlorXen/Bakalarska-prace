using HOR0552.ViewModels;
using HOR0552.Views;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace HOR0552
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkit();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainPageViewModel>();

            builder.Services.AddSingleton<CalendarPage>();
            builder.Services.AddSingleton<CalendarViewModel>();

            builder.Services.AddTransient<DiagnosisDetailPage>();
            builder.Services.AddTransient<DiagnosisDetailViewModel>();

            builder.Services.AddTransient<SelectDiagnosisPage>();
            builder.Services.AddTransient<SelectDiagnosisViewModel>();

            builder.Services.AddTransient<AddEventPage>();
            builder.Services.AddTransient<AddEventViewModel>();

            builder.Services.AddTransient<EventDetailsPage>();
            builder.Services.AddTransient<EventDetailsViewModel>();

            builder.Services.AddTransient<EditEventPage>();
            builder.Services.AddTransient<EditEventViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<App>();
            var app = builder.Build();
            
            var appInstance = app.Services.GetService<App>();
            if (appInstance != null)
            {
                appInstance.PreloadCalendarPage(app.Services);
            }
            else
            {
                Console.WriteLine("App instance is null");
            }
            
            return app;
        }
    }
}