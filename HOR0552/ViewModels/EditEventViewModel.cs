using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HOR0552.Models;
using HOR0552.Views;

namespace HOR0552.ViewModels
{
    [QueryProperty(nameof(EditedEvent), "EditedEvent")]
    public partial class EditEventViewModel : ObservableObject
    {
        [ObservableProperty]
        CalendarEvent editedEvent;

        [ObservableProperty]
        DateTime selectedDate;

        [ObservableProperty]
        string eventTitle;

        [ObservableProperty]
        bool isAllDay;

        [ObservableProperty]
        DateTime eventDate;

        [ObservableProperty]
        TimeSpan eventTime;

        [ObservableProperty]
        string location;

        [ObservableProperty]
        string notes;

        [ObservableProperty]
        string selectedColor = "Modrá";

        [ObservableProperty]
        Diagnosis? selectedDiagnosis;

        [ObservableProperty]
        ObservableCollection<Diagnosis> diagnoses;

        ObservableCollection<CalendarEvent> eventCollection;
        async public void OnPageAppearing()
        {
                EventTitle = EditedEvent.name;
                IsAllDay = EditedEvent.date.TimeOfDay == TimeSpan.Zero;
                EventDate = EditedEvent.date;
                EventTime = EditedEvent.date.TimeOfDay;
                Location = EditedEvent.location;
                Notes = EditedEvent.description;
                SelectedDate = EventDate.Date.Add(EventTime);
            string clr;
                switch (EditedEvent.color)
                {
                    case "Blue":
                        clr = "Modrá";
                        break;
                    case "Red":
                        clr = "Červená";
                        break;
                    case "Green":
                        clr = "Zelená";
                        break;
                    case "Yellow":
                        clr = "Žlutá";
                        break;
                    case "Magenta":
                        clr = "Fialová";
                        break;
                    default:
                        clr = "Modrá";
                        break;
                }

                SelectedColor = clr;

            LoadSelectedDiagnoses();

            SelectedDiagnosis = null;

            foreach (Diagnosis diagnosis in Diagnoses)
            {
                if(diagnosis.diagnosisId.Equals(EditedEvent.diagnosisId))
                {
                    SelectedDiagnosis = diagnosis;
                    break;
                }
            }

            LoadAllEvents();
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
                    Diagnoses.Add(new Diagnosis { diagnosisId = "", name = "Bez diagnózy", currentStepNum = 0, startDate = null, treatmentPlan = new List<TreatmentStep>() });
                }
            }
            else
            {
                Diagnoses = new ObservableCollection<Diagnosis>();
                Diagnoses.Add(new Diagnosis { diagnosisId = "", name = "Bez diagnózy", currentStepNum = 0, startDate = null, treatmentPlan = new List<TreatmentStep>()});
            }
        }

        [RelayCommand]
        async Task EditEvent()
        {
            string clr;
            switch (SelectedColor)
            {
                case "Modrá":
                    clr = "Blue";
                    break;
                case "Červená":
                    clr = "Red";
                    break;
                case "Zelená":
                    clr = "Green";
                    break;
                case "Žlutá":
                    clr = "Yellow";
                    break;
                case "Fialová":
                    clr = "Magenta";
                    break;
                default:
                    clr = "Blue";
                    break;
            }

            CalendarEvent newEvent = new CalendarEvent
            {
                diagnosisId = (SelectedDiagnosis == null) ? null : SelectedDiagnosis.diagnosisId,
                diagnosisName = (SelectedDiagnosis == null) ? null : SelectedDiagnosis.name,
                name = EventTitle,
                date = (IsAllDay) ? EventDate : (EventDate.Add(EventTime)),
                location = Location,
                description = Notes,
                color = clr
            };

            for (int i = 0; i < eventCollection.Count; i++)
            {
                var currentEvent = eventCollection[i];
                if (currentEvent.diagnosisId == EditedEvent.diagnosisId &&
                    currentEvent.diagnosisName == EditedEvent.diagnosisName &&
                    currentEvent.name == EditedEvent.name &&
                    currentEvent.date == EditedEvent.date &&
                    currentEvent.location == EditedEvent.location &&
                    currentEvent.description == EditedEvent.description &&
                    currentEvent.color == EditedEvent.color)
                {
                    eventCollection[i] = newEvent;
                    break;
                }
            }

            updateAllEvents();
            
                await Shell.Current.GoToAsync(nameof(EventDetailsPage), false,
                    new Dictionary<string, object> { { "SelectedEvent", newEvent } });

            var navigationStack = Shell.Current.Navigation.NavigationStack;
            var editEventPage = navigationStack.FirstOrDefault(page => page is EditEventPage);
            var eventDetailsPage = navigationStack.FirstOrDefault(page => page is EventDetailsPage);
            if (editEventPage != null)
            {
                Shell.Current.Navigation.RemovePage(editEventPage);
            }
            if (eventDetailsPage != null)
            {
                Shell.Current.Navigation.RemovePage(eventDetailsPage);
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

            using var writer = new StreamWriter(filePath);
            writer.Write(json);
        }
    }
}