using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HOR0552.Models;
using HOR0552.Views;
using Plugin.Maui.Calendar.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Formats.Asn1;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;

namespace HOR0552.ViewModels;

[QueryProperty(nameof(SelectedEvent), "SelectedEvent")]
public partial class EventDetailsViewModel : ObservableObject
{
    [ObservableProperty]
    private CalendarEvent selectedEvent;

    [ObservableProperty]
    private string formatedDate;

    ObservableCollection<CalendarEvent> eventCollection;

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

    private void LoadAllEvents()
    {
        var filePath = Path.Combine(FileSystem.AppDataDirectory, "events.json");
        if (File.Exists(filePath))
        {
            using var reader = new StreamReader(filePath);
            var json = reader.ReadToEnd();
            if (json != "")
            {
                var loadedEvents = JsonSerializer.Deserialize<ObservableCollection<CalendarEvent>>(json);
                if (loadedEvents != null)
                {
                    eventCollection = loadedEvents;
                }
                else
                {
                    eventCollection = new ObservableCollection<CalendarEvent>();
                }
            }
            else
            {
                eventCollection = new ObservableCollection<CalendarEvent>();
            }

        }
        else
        {
            eventCollection = new ObservableCollection<CalendarEvent>();
        }
    }

    private void updateAllEvents()
    {
        var filePath = Path.Combine(FileSystem.AppDataDirectory, "events.json");
        var json = JsonSerializer.Serialize(eventCollection);

        using (var writer = new StreamWriter(filePath))
        {
            writer.Write(json);
        }
    }

    [RelayCommand]
    async Task DeleteEvent()
    {
        LoadAllEvents();

        for (int i = 0; i < eventCollection.Count; i++)
        {
            var currentEvent = eventCollection[i];
            if (currentEvent.diagnosisId == SelectedEvent.diagnosisId &&
                currentEvent.diagnosisName == SelectedEvent.diagnosisName &&
                currentEvent.name == SelectedEvent.name &&
                currentEvent.date == SelectedEvent.date &&
                currentEvent.location == SelectedEvent.location &&
                currentEvent.description == SelectedEvent.description &&
                currentEvent.color == SelectedEvent.color)
            {
                eventCollection.RemoveAt(i);
                break;
            }
        }

        updateAllEvents();

        await Shell.Current.GoToAsync("..", false);
    }

    [RelayCommand]
    async Task EditEvent()
    {
        await Shell.Current.GoToAsync(nameof(EditEventPage), true,
            new Dictionary<string, object> { { "EditedEvent", SelectedEvent } });
    }
}

