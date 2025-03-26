using CommunityToolkit.Mvvm.ComponentModel;
using HOR0552.Models;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.ConstrainedExecution;

namespace HOR0552.ViewModels;

[QueryProperty(nameof(SelectedEvent), "SelectedEvent")]
public partial class EventDetailsViewModel : ObservableObject
{
    [ObservableProperty]
    private CalendarEvent selectedEvent;

    [ObservableProperty]
    private string formatedDate;


    public EventDetailsViewModel()
    {
        FormatedDate = "";
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName == "SelectedEvent")
        {
            var culture = new CultureInfo("cs-CZ");
            if (SelectedEvent.date.TimeOfDay == TimeSpan.Zero)
            {
                FormatedDate = culture.DateTimeFormat.GetDayName(SelectedEvent.date.DayOfWeek) + " " + SelectedEvent.date.ToString("d");
            }
            else
            {
                FormatedDate = culture.DateTimeFormat.GetDayName(SelectedEvent.date.DayOfWeek) + " " + SelectedEvent.date.ToString("d") + " - " + SelectedEvent.date.ToString("t");
            }


        }
    }
}

