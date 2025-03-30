using CommunityToolkit.Mvvm.ComponentModel;
using HOR0552.Models;
using System.Collections.ObjectModel;


namespace HOR0552.ViewModels;
[QueryProperty(nameof(SelectedDiagnosis), "SelectedDiagnosis")]
public partial class DiagnosisStepsViewModel : ObservableObject
{
    [ObservableProperty]
    Diagnosis selectedDiagnosis;


    public DiagnosisStepsViewModel()
    {
        SelectedDiagnosis = new Diagnosis();
    }
    public void OnPageAppearing()
    {
    }

}
