using CommunityToolkit.Mvvm.ComponentModel;
using Plugin.Maui.Calendar.Models;
using HOR0552.Models;
using HOR0552.Views;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.Input;
using System.Runtime.ConstrainedExecution;

namespace HOR0552.ViewModels;

public partial class CalendarViewModel : ObservableObject
{
    ObservableCollection<CalendarEvent> eventCollection;
    public EventCollection Events { get; set; }
    public CultureInfo Culture => new CultureInfo("cs-CZ");

    [ObservableProperty]
    private bool isAddEventButtonVisible;

    [ObservableProperty]
    private DateTime selectedDate;

    public CalendarViewModel()
    {
        Events = new EventCollection { };

        selectedDate = DateTime.Now;
        isAddEventButtonVisible = false;
        eventCollection = new ObservableCollection<CalendarEvent>();
    }

    public void OnPageAppearing()
    {
        LoadAllEvents();

        Events.Clear();

        foreach (CalendarEvent e in eventCollection)
        {
            Color clr;
            switch (e.color)
            {
                case "Modrá":
                    clr = Colors.Blue;
                    break;
                case "Červená":
                    clr = Colors.Red;
                    break;
                case "Zelená":
                    clr = Colors.Green;
                    break;
                case "Žlutá":
                    clr = Colors.Yellow;
                    break;
                case "Fialová":
                    clr = Colors.Magenta;
                    break;
                default:
                    clr = Colors.Blue;
                    break;
            }

            if (!Events.ContainsKey(e.date))
            {
                Events.Add(e.date, new DayEventCollection<CalendarEvent>(new List<CalendarEvent> { new CalendarEvent { diagnosisId = e.diagnosisId, name = e.name, date = e.date, location = e.location, description = e.description, color = e.color } })
                {
                    EventIndicatorColor = clr,
                    EventIndicatorSelectedColor = clr,
                    EventIndicatorSelectedTextColor = clr
                });
            }
            else if (Events[e.date] is DayEventCollection<CalendarEvent> eventList)
            {
                eventList.Add(new CalendarEvent { diagnosisId = e.diagnosisId, name = e.name, date = e.date, location = e.location, description = e.description, color = e.color });
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
            if(json != "")
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
            } else
            {
                eventCollection = new ObservableCollection<CalendarEvent>();
            }
            
        }
        else
        {
            eventCollection = new ObservableCollection<CalendarEvent>();
        }
    }

    partial void OnSelectedDateChanged(DateTime value)
    {
        if(value != null)
            IsAddEventButtonVisible = true;
        else
            IsAddEventButtonVisible = false;
    }

    [RelayCommand]
    async Task AddEvent()
    {
        await Shell.Current.GoToAsync(nameof(AddEventPage), true,
            new Dictionary<string, object> { { "SelectedDate", SelectedDate } });
    }
}
