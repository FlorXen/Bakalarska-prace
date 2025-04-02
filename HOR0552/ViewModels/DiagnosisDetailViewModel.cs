using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HOR0552.Models;
using HOR0552.Views;
using HOR0552.ViewModels;
using System.Collections.ObjectModel;
using System.Text.Json;
using Plugin.Maui.Calendar.Models;

namespace HOR0552.ViewModels;
[QueryProperty(nameof(SelectedDiagnosis), "SelectedDiagnosis")]
public partial class DiagnosisDetailViewModel : ObservableObject
{
    [ObservableProperty]
    Diagnosis selectedDiagnosis;

    [ObservableProperty]
    TreatmentStep currentStep;

    [ObservableProperty]
    bool isConfirmStepButtonVisible = true;

    private bool isDiagnosisFinished = false;
    public void OnPageAppearing()
    {
        loadSelectedDiagnoses();

        foreach(TreatmentStep treatmentStep in SelectedDiagnosis.treatmentPlan)
        {
            if (treatmentStep.step == SelectedDiagnosis.currentStepNum)
            {
                CurrentStep = treatmentStep;
                if (CurrentStep.isCompleted)
                {
                    isDiagnosisFinished = true;
                }
                break;
            }
        }

        if(isDiagnosisFinished)
        {
            IsConfirmStepButtonVisible = false;
        }
    }

    private void loadSelectedDiagnoses()
    {
        ObservableCollection<Diagnosis> Diagnoses = new ObservableCollection<Diagnosis>();

        // Načtení díagnóz

        var filePath = Path.Combine(FileSystem.AppDataDirectory, "selected_diagnoses.json");
        if (File.Exists(filePath))
        {
            using var reader = new StreamReader(filePath);
            var json = reader.ReadToEnd();
            if (json != "")
            {
                var loadedDiagnoses = JsonSerializer.Deserialize<ObservableCollection<Diagnosis>>(json);
                if (loadedDiagnoses != null && loadedDiagnoses.Count > 0)
                {
                    Diagnoses = loadedDiagnoses;
                }
                else
                {
                    Diagnoses = new ObservableCollection<Diagnosis>();
                }
            }
            else
            {
                Diagnoses = new ObservableCollection<Diagnosis>();
            }
        }
        else
        {
            Diagnoses = new ObservableCollection<Diagnosis>();
        }

        if (Diagnoses != null && Diagnoses.Count > 0)
        {
            for (int i = 0; i < Diagnoses.Count; i++)
            {
                if (Diagnoses[i].diagnosisId == SelectedDiagnosis.diagnosisId)
                {
                    SelectedDiagnosis = Diagnoses[i];
                    if (SelectedDiagnosis.treatmentPlan != null && SelectedDiagnosis.treatmentPlan.Count > 0)
                    {
                        foreach (TreatmentStep treatmentStep in SelectedDiagnosis.treatmentPlan)
                        {
                            if (!treatmentStep.isCompleted && treatmentStep.stepDate != null)
                            {
                                treatmentStep.daysUntilDeadline = (int)(treatmentStep.stepDate.Value.AddDays(treatmentStep.deadlineInDays) - DateTime.Now.Date).TotalDays;
                                CurrentStep = treatmentStep;
                                break;
                            }
                        }
                    }
                    break;
                }
            }
        }
    }
    private void updateSelectedDiagnosis()
    {
        ObservableCollection<Diagnosis> Diagnoses = new ObservableCollection<Diagnosis>();

        // Načtení díagnóz

        var filePathRead = Path.Combine(FileSystem.AppDataDirectory, "selected_diagnoses.json");
        if (File.Exists(filePathRead))
        {
            using var reader = new StreamReader(filePathRead);
            var jsonReader = reader.ReadToEnd();
            var loadedDiagnoses = JsonSerializer.Deserialize<ObservableCollection<Diagnosis>>(jsonReader);
            if (loadedDiagnoses != null)
            {
                Diagnoses = loadedDiagnoses;
            }
        }

        // Aktualizace díagnózy
        if (Diagnoses != null && Diagnoses.Count > 0)
        {
            for (int i = 0; i < Diagnoses.Count; i++)
            {
                if (Diagnoses[i].diagnosisId == SelectedDiagnosis.diagnosisId)
                {
                    if (SelectedDiagnosis.treatmentPlan != null && SelectedDiagnosis.treatmentPlan.Count > 0)
                    {
                        foreach (TreatmentStep treatmentStep in SelectedDiagnosis.treatmentPlan)
                        {
                            if (!treatmentStep.isCompleted && treatmentStep.stepDate != null)
                            {
                                treatmentStep.daysUntilDeadline = (int)(treatmentStep.stepDate.Value.AddDays(treatmentStep.deadlineInDays) - DateTime.Now.Date).TotalDays;
                                CurrentStep = treatmentStep;
                                break;
                            }
                        }
                    }
                    Diagnoses[i] = SelectedDiagnosis;
                    break;
                }
            }
        }

        // Uložení díagnóz

        var filePathWrite = Path.Combine(FileSystem.AppDataDirectory, "selected_diagnoses.json");
        var jsonWriter = JsonSerializer.Serialize(Diagnoses);

        using (var writer = new StreamWriter(filePathWrite))
        {
            writer.Write(jsonWriter);
        }
    }

    [RelayCommand]
    private void ConfirmStep()
    {
        int? nextStepNum = null;

        foreach (TreatmentStep step in SelectedDiagnosis.treatmentPlan)
        {
            if (nextStepNum == null || nextStepNum == step.step)
            {
                if (step.isCompleted)
                {
                    // Krok je dokončený a má 1 následující krok
                    if (step.nextStep != null)
                    {
                        nextStepNum = step.nextStep;
                        continue;
                    }
                    else // Krok je dokončený, ale nemá další 1 krok
                    {
                        break;
                    }
                }
                else
                {
                    // Krok není dokončený a nemá 1 následující krok
                    if (step.nextStep == null)
                    {
                        //Krok není dokončený a nemá více následujících kroků
                        if (step.nextSteps == null)
                        {
                            // Léčba je dokončená
                            step.isCompleted = true;
                        }
                        else // Krok není dokončený a má více následujících kroků
                        {
                            // Volba z jednoho z dalších kroků
                            var popup = new StepResultPopup(new StepResultPopupViewModel(step, SelectedDiagnosis.diagnosisId), loadSelectedDiagnoses);
                            Shell.Current.CurrentPage.ShowPopup(popup);
                            nextStepNum = null;
                        }
                    }
                    else // Krok není dokončený a má 1 následující krok
                    {
                        step.isCompleted = true;
                        SelectedDiagnosis.currentStepNum = (int)step.nextStep;
                        nextStepNum = 0;
                    }

                    break;
                }
            }
        }

        if (nextStepNum != null)
        {
            foreach (TreatmentStep step in SelectedDiagnosis.treatmentPlan)
            {
                if (step.step == SelectedDiagnosis.currentStepNum)
                {
                    if(step.isCompleted)
                    {
                        if (CurrentStep.isCompleted)
                        {
                            isDiagnosisFinished = true;
                            IsConfirmStepButtonVisible = false;
                            CurrentStep = null;
                            CurrentStep = step;
                        }
                    }
                    else
                    {
                        step.stepDate = DateTime.Now.Date;
                        step.daysUntilDeadline = step.deadlineInDays;
                    }
                    
                    break;
                }
            }
            updateSelectedDiagnosis();
        }

        // Vynucení aktualizace DeadlineBaru
        OnPropertyChanged(nameof(SelectedDiagnosis));
    }

    [RelayCommand]
    async Task ViewAllSteps()
    {
        await Shell.Current.GoToAsync(nameof(DiagnosisStepsPage), true,
            new Dictionary<string, object> { { "SelectedDiagnosis", SelectedDiagnosis } });
    }
    [RelayCommand]
    async Task ViewCalendar()
    {
        await Shell.Current.GoToAsync(nameof(DiagnosisDetailCalendarPage), true,
            new Dictionary<string, object> { { "SelectedDiagnosis", SelectedDiagnosis } });
    }

    [RelayCommand]
    async Task DeleteDiagnosis()
    {
        ObservableCollection<Diagnosis> Diagnoses = new ObservableCollection<Diagnosis>();

        // Načtení díagnóz

        var filePathRead = Path.Combine(FileSystem.AppDataDirectory, "selected_diagnoses.json");
        if (File.Exists(filePathRead))
        {
            using var reader = new StreamReader(filePathRead);
            var jsonReader = reader.ReadToEnd();
            var loadedDiagnoses = JsonSerializer.Deserialize<ObservableCollection<Diagnosis>>(jsonReader);
            if (loadedDiagnoses != null)
            {
                Diagnoses = loadedDiagnoses;
            }
        }

        // Odstranění díagnózy ze seznamu
        if(Diagnoses != null)
        for (int i = 0; i < Diagnoses.Count; i++)
        {
            if (Diagnoses[i].diagnosisId == SelectedDiagnosis.diagnosisId)
            {
                Diagnoses.RemoveAt(i);
                break;
            }
        }

        // Uložení díagnóz

        var filePathWrite = Path.Combine(FileSystem.AppDataDirectory, "selected_diagnoses.json");
        var jsonWriter = JsonSerializer.Serialize(Diagnoses);

        using (var writer = new StreamWriter(filePathWrite))
        {
            writer.Write(jsonWriter);
        }

        await Shell.Current.GoToAsync("..", true);
    }
}
