using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HOR0552.Models;

namespace HOR0552.ViewModels
{
    [QueryProperty(nameof(SelectedDate), "SelectedDate")]
    public partial class AddEventViewModel : ObservableObject
    {
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
        Diagnosis selectedDiagnosis;

        [ObservableProperty]
        ObservableCollection<Diagnosis> diagnoses;

        ObservableCollection<CalendarEvent> eventCollection;
        public void OnPageAppearing()
        {
            EventDate = SelectedDate;
            EventTime = DateTime.Now.TimeOfDay;
            LoadSelectedDiagnoses();
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
                }
            }
            else
            {
                Diagnoses = new ObservableCollection<Diagnosis>();
            }
        }

        [RelayCommand]
        private void AddEvent()
        {
            eventCollection.Add(new CalendarEvent { diagnosisId =(selectedDiagnosis == null) ? null : selectedDiagnosis.diagnosisId, name = eventTitle, date = (IsAllDay)?EventDate:(EventDate.Add(EventTime)), location = Location, description = Notes, color = SelectedColor });
            
            updateAllEvents();

            Shell.Current.GoToAsync("..");
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