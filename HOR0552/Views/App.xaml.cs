﻿using System.Globalization;
using HOR0552.ViewModels;
using HOR0552.Views;

namespace HOR0552
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("cs-CZ");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("cs-CZ");
            Microsoft.Maui.Handlers.DatePickerHandler.Mapper.PrependToMapping("MyCustomization", (handler, view) =>
            {
#if ANDROID
                        Java.Util.Locale locale = new Java.Util.Locale("cs-CZ");
                        handler.PlatformView.TextLocale = locale;
                        Android.Content.Res.Configuration config = new Android.Content.Res.Configuration();
                        config.Locale = locale;
                        Java.Util.Locale.SetDefault(Java.Util.Locale.Category.Format, locale);
                        Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.Resources.Configuration.SetLocale(locale);
                        Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.Resources.Configuration.Locale = locale;
                        Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.Resources.UpdateConfiguration(config, Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.Resources.DisplayMetrics);
                        Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.Resources.UpdateConfiguration(config, Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.Resources.DisplayMetrics);
#endif
            });
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());
            return window;
        }

        public void PreloadCalendarPage(IServiceProvider services)
        {
            Task.Run(async () =>
            {
                var calendarPage = services.GetService<CalendarPage>();
                if (calendarPage != null)
                {
                    // Předvykreslení stránky kalendáře
                    await Application.Current.Dispatcher.DispatchAsync(() =>
                    {
                        var content = calendarPage.Content;
                    });
                }
            });
        }
    }
}