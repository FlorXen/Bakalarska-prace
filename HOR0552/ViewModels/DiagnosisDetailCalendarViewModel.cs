using CommunityToolkit.Mvvm.ComponentModel;
using Plugin.Maui.Calendar.Models;
using HOR0552.Models;
using HOR0552.Views;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.Input;

namespace HOR0552.ViewModels;

[QueryProperty(nameof(SelectedDiagnosis), "SelectedDiagnosis")]
public partial class DiagnosisDetailCalendarViewModel : ObservableObject
{
    ObservableCollection<Diagnosis> Diagnoses;
    ObservableCollection<CalendarEvent> eventCollection;
    public EventCollection Events { get; set; }

    [ObservableProperty]
    public CultureInfo culture;

    [ObservableProperty]
    private Diagnosis? selectedDiagnosis;

    [ObservableProperty]
    private bool isAddEventButtonVisible;

    [ObservableProperty]
    private DateTime? selectedDate;

    [ObservableProperty]
    private DateTime displayDate;
    public DiagnosisDetailCalendarViewModel()
    {
        Culture = new CultureInfo("cs-CZ");
        Events = new EventCollection { };
        Diagnoses = new ObservableCollection<Diagnosis>();
        SelectedDate = DateTime.Now;
        DisplayDate = DateTime.Now;
        IsAddEventButtonVisible = false;
        eventCollection = new ObservableCollection<CalendarEvent>();
    }

    public void OnPageAppearing()
    {
        SelectedDate = DateTime.Now;
        LoadAllEvents();
        PopulateEvents();
    }

    public void PopulateEvents()
    {
        Events.Clear();

        if (SelectedDiagnosis == null)
            return;
        foreach (TreatmentStep treatmentStep in SelectedDiagnosis.treatmentPlan)
        {
            if (!treatmentStep.isCompleted && treatmentStep.stepDate != null)
            {
                DateTime deadlineDate = DateTime.Now.Date.AddDays(treatmentStep.daysUntilDeadline);

                if (deadlineDate.Month != DisplayDate.Month || deadlineDate.Year != DisplayDate.Year)
                {
                    // Skip adding events that are not in the current month
                    continue;
                }

                if (!Events.ContainsKey(deadlineDate))
                {
                    Events.Add(deadlineDate, new DayEventCollection<CalendarEvent>(new List<CalendarEvent> { new CalendarEvent {
                            diagnosisId = SelectedDiagnosis.diagnosisId,
                            diagnosisName = SelectedDiagnosis.name,
                            name = "Konečný termín pro krok " + treatmentStep.procedure.name,
                            date = deadlineDate,
                            location = "",
                            description = "",
                            color = "Red" } })
                    {
                        EventIndicatorColor = Colors.Red,
                        EventIndicatorSelectedColor = Colors.Red,
                        EventIndicatorSelectedTextColor = Colors.Red,
                        Colors = [Colors.Red]
                    });
                }
                else if (Events[deadlineDate] is DayEventCollection<CalendarEvent> eventList)
                {
                    eventList.Add(new CalendarEvent
                    {
                        diagnosisId = SelectedDiagnosis.diagnosisId,
                        diagnosisName = SelectedDiagnosis.name,
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
    

        foreach (CalendarEvent e in eventCollection)
        {

            if (!SelectedDiagnosis.diagnosisId.Equals(e.diagnosisId))
                continue;

            if (e.date.Month != DisplayDate.Month || e.date.Year != DisplayDate.Year)
            {
                // Skip adding events that are not in the current month
                continue;
            }

            if (!Events.ContainsKey(e.date))
            {
                Events.Add(e.date, new DayEventCollection<CalendarEvent>(new List<CalendarEvent> { new CalendarEvent { diagnosisId = e.diagnosisId, diagnosisName = e.diagnosisName, name = e.name, date = e.date, location = e.location, description = e.description, color = e.color } })
                {
                    EventIndicatorColor = Color.Parse(e.color),
                    EventIndicatorSelectedColor = Color.Parse(e.color),
                    EventIndicatorSelectedTextColor = Color.Parse(e.color),
                    Colors = [Color.Parse(e.color)]
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

    partial void OnSelectedDateChanged(DateTime? value)
    {
        if (value != null)
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
        PopulateEvents();

    }

    [RelayCommand]
    void NextMonth()
    {
        DisplayDate = DisplayDate.AddMonths(1);
        PopulateEvents();

    }
}
