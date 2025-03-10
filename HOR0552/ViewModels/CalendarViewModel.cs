using CommunityToolkit.Mvvm.ComponentModel;
using Plugin.Maui.Calendar.Models;
using HOR0552.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using HOR0552.Models;
using System.Collections.ObjectModel;
using System.Text.Json;
using Android.Mtp;

namespace HOR0552.ViewModels;

public partial class CalendarViewModel : ObservableObject
{
    ObservableCollection<Diagnosis> diagnoses;
    public EventCollection Events { get; set; }
    public CultureInfo Culture => new CultureInfo("cs-CZ");
    public CalendarViewModel()
    {
        LoadSelectedDiagnoses();
        
                Events = new EventCollection { };

                foreach (Diagnosis diagnosis in diagnoses)
                {
                    foreach (TreatmentStep treatmentStep in diagnosis.treatmentPlan)
                    {
                        if (treatmentStep.step == diagnosis.currentStepNum)
                        {
                            var eventDate = treatmentStep.stepDate.Value.AddDays(treatmentStep.deadlineInDays);
                    /*
                            if (!Events.ContainsKey(eventDate))
                            {
                                Events[eventDate] = new DayEventCollection<TreatmentStepEvent>();
                            }

                            if (Events[eventDate] is DayEventCollection<TreatmentStepEvent> eventList)
                            {
                                eventList.Add(new TreatmentStepEvent { step = treatmentStep });
                            }
                    */
                            Events.Add(eventDate, new DayEventCollection<TreatmentStepEvent>(new List<TreatmentStepEvent> { new TreatmentStepEvent { step = treatmentStep, name = "Konečný termín kroku: " + treatmentStep.procedure.name } })
                            {
                                EventIndicatorTextColor = Colors.Red,
                                EventIndicatorColor = Colors.Red,
                                EventIndicatorSelectedColor = Colors.Red,
                                EventIndicatorSelectedTextColor = Colors.Red
                            });

                    break;
                        }
                    }
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
                diagnoses = loadedDiagnoses;
            }
        }
        else
        {
            diagnoses = new ObservableCollection<Diagnosis>();
        }
    }

    // Add the missing GenerateEvents method
    private IEnumerable<TreatmentStepEvent> GenerateEvents(int count, string name)
    {
        var events = new List<TreatmentStepEvent>();
        for (int i = 0; i < count; i++)
        {
            events.Add(new TreatmentStepEvent { name = name });
        }
        return events;
    }
}
