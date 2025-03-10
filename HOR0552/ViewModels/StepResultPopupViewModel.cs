using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HOR0552.Models;
using HOR0552.Views;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace HOR0552.ViewModels;
public partial class StepResultPopupViewModel : ObservableObject
{
    [ObservableProperty]
    TreatmentStep step;

    string diagnosisId;
    ObservableCollection<Diagnosis> diagnoses;

    public StepResultPopupViewModel(TreatmentStep step, string diagnosisId)
    {
        this.step = step;
        this.diagnosisId = diagnosisId;
    }

    [RelayCommand]
    async Task SelectResult(int? nextStep)
    {
        LoadSelectedDiagnoses();

        foreach (Diagnosis diagnosis in diagnoses)
        {
            if (diagnosis.diagnosisId == diagnosisId)
            {
                foreach (TreatmentStep treatmentStep in diagnosis.treatmentPlan)
                {
                    if (treatmentStep.step == Step.step)
                    {
                        treatmentStep.isCompleted = true;
                        treatmentStep.nextStep = nextStep;
                        diagnosis.currentStepNum = nextStep;
                        break;
                    }
                }

                foreach (TreatmentStep treatmentStep in diagnosis.treatmentPlan)
                {
                    if (treatmentStep.step == nextStep)
                    {
                        treatmentStep.stepDate = DateTime.Now.Date;
                        treatmentStep.daysUntilDeadline = treatmentStep.deadlineInDays;
                        break;
                    }
                }
                break;
            }
        }

        updateSelectedDiagnoses();
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
