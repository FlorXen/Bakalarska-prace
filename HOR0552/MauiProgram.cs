﻿using HOR0552.ViewModels;
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

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}