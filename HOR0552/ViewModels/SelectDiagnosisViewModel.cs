using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HOR0552.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using HOR0552.Views;
using CommunityToolkit.Maui.Views;

namespace HOR0552.ViewModels;
public partial class SelectDiagnosisViewModel : ObservableObject
{

    [ObservableProperty]
    ObservableCollection<Diagnosis> diagnoses;

    [ObservableProperty]
    ObservableCollection<Diagnosis> filteredDiagnoses;
    public SelectDiagnosisViewModel()
    {
        LoadAllDiagnoses();

        filteredDiagnoses = diagnoses;
    }
    [RelayCommand]
    async void ShowDetail(Diagnosis diagnosis)
    {
        var popup = new DiagnosisDetailPopup(new DiagnosisDetailPopupViewModel(diagnosis));
        Shell.Current.CurrentPage.ShowPopup(popup);
    }

    private async void LoadAllDiagnoses()
    {
        var assembly = typeof(MainPageViewModel).Assembly;
        using var stream = assembly.GetManifestResourceStream("HOR0552.Resources.Raw.diagnoses.json");
        if (stream == null)
        {
            throw new FileNotFoundException("Could not find the embedded resource 'HOR0552.Resources.Raw.diagnoses.json'");
        }

        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        var diagnosesRootJson = JsonSerializer.Deserialize<DiagnosesRoot>(json);
        if (diagnosesRootJson?.diagnoses != null)
        {
            DiagnosesRoot diagnosesRoot = diagnosesRootJson;
            Diagnoses = new ObservableCollection<Diagnosis>(diagnosesRoot.diagnoses);
        }
        else
        {
            Diagnoses = new ObservableCollection<Diagnosis>();
        }
    }

    public void FilterDiagnoses(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            FilteredDiagnoses = new ObservableCollection<Diagnosis>(Diagnoses);
        }
        else
        {
            var filteredList = Diagnoses
                .Where(d => d.name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .ToList();
            FilteredDiagnoses = new ObservableCollection<Diagnosis>(filteredList);
        }
    }
}

