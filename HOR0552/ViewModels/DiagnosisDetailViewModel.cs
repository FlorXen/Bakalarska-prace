using CommunityToolkit.Mvvm.ComponentModel;
using HOR0552.Models;

namespace HOR0552.ViewModels;
[QueryProperty(nameof(SelectedDiagnosis), "SelectedDiagnosis")]
public partial class DiagnosisDetailViewModel : ObservableObject
{
    [ObservableProperty]
    Diagnosis selectedDiagnosis;
}
