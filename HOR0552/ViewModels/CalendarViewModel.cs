using CommunityToolkit.Mvvm.ComponentModel;
using Plugin.Maui.Calendar.Models;
using HOR0552.Models;
using HOR0552.Views;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.Input;

namespace HOR0552.ViewModels;
public partial class CalendarViewModel : ObservableObject
{
    ObservableCollection<Diagnosis> Diagnoses;
    ObservableCollection<CalendarEvent> eventCollection;
    public EventCollection Events { get; set; }
    public CultureInfo Culture => new CultureInfo("cs-CZ");

    [ObservableProperty]
    private bool isAddEventButtonVisible;

    [ObservableProperty]
    private DateTime? selectedDate;

    [ObservableProperty]
    private DateTime displayDate;
    public CalendarViewModel()
    {
        Events = new EventCollection { };
        SelectedDate = DateTime.Now;
        DisplayDate = DateTime.Now;
        IsAddEventButtonVisible = false;
        eventCollection = new ObservableCollection<CalendarEvent>();
    }
    
    public void OnPageAppearing()
    {
        SelectedDate = null;
        LoadAllEvents();
        LoadSelectedDiagnoses();
        PopulateEvents();
    }

    public void PopulateEvents()
    {
        Events.Clear();

        foreach (Diagnosis diagnosis in Diagnoses)
        {
            foreach (TreatmentStep treatmentStep in diagnosis.treatmentPlan)
            {
                if (!treatmentStep.isCompleted && treatmentStep.stepDate != null)
                {
                    DateTime deadlineDate = DateTime.Now.Date.AddDays(treatmentStep.daysUntilDeadline);

                    if (!Events.ContainsKey(deadlineDate))
                    {
                        Events.Add(deadlineDate, new DayEventCollection<CalendarEvent>(new List<CalendarEvent> { new CalendarEvent { diagnosisId = diagnosis.diagnosisId,
                            diagnosisName = diagnosis.name,
                            name = "Konečný termín pro krok " + treatmentStep.procedure.name,
                            date = deadlineDate,
                            location = "",
                            description = "",
                            color = "Red" } })
                        {
                            EventIndicatorColor = Colors.Red,
                            EventIndicatorSelectedColor = Colors.Red,
                            EventIndicatorSelectedTextColor = Colors.Red
                        });
                    }
                    else if (Events[deadlineDate] is DayEventCollection<CalendarEvent> eventList)
                    {
                        eventList.Add(new CalendarEvent
                        {
                            diagnosisId = diagnosis.diagnosisId,
                            diagnosisName = diagnosis.name,
                            name = "Konečný termín pro krok " + treatmentStep.procedure.name,
                            date = deadlineDate,
                            location = "",
                            description = "",
                            color = "Red"
                        });
                    }

                    break;
                }
            }
        }

        foreach (CalendarEvent e in eventCollection)
        {
            Color clr;
            switch (e.color)
            {
                case "Blue":
                    clr = Colors.Blue;
                    break;
                case "Red":
                    clr = Colors.Red;
                    break;
                case "Green":
                    clr = Colors.Green;
                    break;
                case "Yellow":
                    clr = Colors.Yellow;
                    break;
                case "Magenta":
                    clr = Colors.Magenta;
                    break;
                default:
                    clr = Colors.Blue;
                    break;
            }

            if (!Events.ContainsKey(e.date))
            {
                Events.Add(e.date, new DayEventCollection<CalendarEvent>(new List<CalendarEvent> { new CalendarEvent { diagnosisId = e.diagnosisId, diagnosisName = e.diagnosisName, name = e.name, date = e.date, location = e.location, description = e.description, color = e.color } })
                {
                    EventIndicatorColor = clr,
                    EventIndicatorSelectedColor = clr,
                    EventIndicatorSelectedTextColor = clr
                });
            }
            else if (Events[e.date] is DayEventCollection<CalendarEvent> eventList)
            {
                eventList.Add(new CalendarEvent { diagnosisId = e.diagnosisId, diagnosisName = e.diagnosisName, name = e.name, date = e.date, location = e.location, description = e.description, color = e.color });
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

    private void LoadSelectedDiagnoses()
    {
        var filePath = Path.Combine(FileSystem.AppDataDirectory, "selected_diagnoses.json");
        if (File.Exists(filePath))
        {
            using var reader = new StreamReader(filePath);
            var json = reader.ReadToEnd();
            var loadedDiagnoses = JsonSerializer.Deserialize<ObservableCollection<Diagnosis>>(json);
            if (loadedDiagnoses != null)
            {
                Diagnoses = loadedDiagnoses;
            }
        }
        else
        {
            Diagnoses = new ObservableCollection<Diagnosis>();
        }
    }

    partial void OnSelectedDateChanged(DateTime? value)
    {
        if(value != null)
        {
            IsAddEventButtonVisible = true;
        }
        else
        {
            IsAddEventButtonVisible = false;
        }
    }

    [RelayCommand]
    async Task AddEvent()
    {
        await Shell.Current.GoToAsync(nameof(AddEventPage), true,
            new Dictionary<string, object> { { "SelectedDate", SelectedDate } });
    }

    [RelayCommand]
    async Task EventTapped(CalendarEvent calendarEvent)
    {
        if (calendarEvent != null)
        {
            await Shell.Current.GoToAsync(nameof(EventDetailsPage), false,
                new Dictionary<string, object> { { "SelectedEvent", calendarEvent } });
        }
    }

    [RelayCommand]
    void PreviousMonth()
    {
        DisplayDate = DisplayDate.AddMonths(-1);
    }

    [RelayCommand]
    void NextMonth()
    {
        DisplayDate = DisplayDate.AddMonths(1);
    }
}
