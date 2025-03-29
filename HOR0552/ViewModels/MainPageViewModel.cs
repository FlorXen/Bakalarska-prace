using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HOR0552.Models;
using HOR0552.Views;

namespace HOR0552.ViewModels;
[QueryProperty(nameof(NewSelectedDiagnosis), "NewSelectedDiagnosis")]
public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    ObservableCollection<Diagnosis> diagnoses;
    [ObservableProperty]
    Diagnosis? newSelectedDiagnosis;
    Diagnosis? _LastNewSelectedDiagnosis;

    public void OnPageAppearing()
    {
        LoadSelectedDiagnoses();
        updateSelectedDiagnoses();
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName == "NewSelectedDiagnosis" && newSelectedDiagnosis != null && newSelectedDiagnosis != _LastNewSelectedDiagnosis)
        {
            _LastNewSelectedDiagnosis = newSelectedDiagnosis;

            newSelectedDiagnosis.startDate = DateTime.Now.Date;
            newSelectedDiagnosis.treatmentPlan[0].stepDate = DateTime.Now.Date;

            diagnoses.Add(newSelectedDiagnosis);
            updateSelectedDiagnoses();
            
        }
    }
    [RelayCommand]
    async Task ShowDetail(Diagnosis diagnosis)
    {/*
        await Shell.Current.GoToAsync(nameof(DiagnosisDetailPage), true,
            new Dictionary<string, object> { { "SelectedDiagnosis", diagnosis } });
        */
        await Shell.Current.GoToAsync(nameof(DiagnosisDetailCalendarPage), true,
            new Dictionary<string, object> { { "SelectedDiagnosis", diagnosis } });
    }

    [RelayCommand]
    private void Delete(Diagnosis diagnosis)
    {
        if (diagnoses.Contains(diagnosis))
        {
            diagnoses.Remove(diagnosis);
            updateSelectedDiagnoses();
        }
    }

    [RelayCommand]
    private void NextStep(Diagnosis diagnosis)
    {
        int? nextStepNum = null;

        foreach (TreatmentStep step in diagnosis.treatmentPlan)
        {
            if(nextStepNum == null || nextStepNum == step.step)
            {
                if (step.isCompleted)
                {
                    // Krok je dokončený a má 1 následující krok
                    if (step.nextStep != null)
                    {
                        nextStepNum = step.nextStep;
                        continue;
                    } else // Krok je dokončený, ale nemá další 1 krok
                    {
                        break;
                    }
                } else
                {
                    // Krok není dokončený a nemá 1 následující krok
                    if (step.nextStep == null)
                    {
                        //Krok není dokončený a nemá více následujících kroků
                        if(step.nextSteps == null)
                        {
                            // Léčba je dokončená
                            step.isCompleted = true;
                        }
                        else // Krok není dokončený a má více následujících kroků
                        {
                            // Volba z jednoho z dalších kroků
                            var popup = new StepResultPopup(new StepResultPopupViewModel(step, diagnosis.diagnosisId), OnPageAppearing);
                            Shell.Current.CurrentPage.ShowPopup(popup);
                            nextStepNum = null;
                        }
                    } else // Krok není dokončený a má 1 následující krok
                    {
                        step.isCompleted = true;
                        diagnosis.currentStepNum = (int)step.nextStep;
                        nextStepNum = 0;
                    }

                    break;
                }
            }
        }

        if(nextStepNum != null)
            foreach (TreatmentStep step in diagnosis.treatmentPlan)
            {
                if (step.step == diagnosis.currentStepNum)
                {
                    step.stepDate = DateTime.Now.Date;
                    step.daysUntilDeadline = step.deadlineInDays;
                    break;
                }
            }
        
        updateSelectedDiagnoses();
        LoadSelectedDiagnoses();
    }

    [RelayCommand]
    async Task AddDiagnosis()
    {
        await Shell.Current.GoToAsync(nameof(SelectDiagnosisPage), true);
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

    private void updateSelectedDiagnoses()
    {
        foreach (Diagnosis diagnosis in diagnoses)
        {
            foreach (TreatmentStep treatmentStep in diagnosis.treatmentPlan)
            {
                if (!treatmentStep.isCompleted && treatmentStep.stepDate != null)
                {
                    treatmentStep.daysUntilDeadline = (int)(treatmentStep.stepDate.Value.AddDays(treatmentStep.deadlineInDays) - DateTime.Now.Date).TotalDays;

                    break;
                }
            }
        }

        var filePath = Path.Combine(FileSystem.AppDataDirectory, "selected_diagnoses.json");
        var json = JsonSerializer.Serialize(diagnoses);

        using var writer = new StreamWriter(filePath);
        writer.Write(json);
    }
}

