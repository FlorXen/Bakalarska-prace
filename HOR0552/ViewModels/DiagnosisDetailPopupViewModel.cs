using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HOR0552.Models;
using System.Collections.ObjectModel;

namespace HOR0552.ViewModels;
public partial class DiagnosisDetailPopupViewModel : ObservableObject
{
    [ObservableProperty]
    Diagnosis diagnosis;

    public DiagnosisDetailPopupViewModel(Diagnosis diagnosis)
    {
        this.diagnosis = diagnosis;
    }

    [RelayCommand]
    async Task SelectDiagnosis()
    {
        await Shell.Current.GoToAsync($"..", true,
            new Dictionary<string, object> { { "NewSelectedDiagnosis", this.diagnosis } });
    }
}
