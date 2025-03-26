using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HOR0552.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using HOR0552.Views;
using CommunityToolkit.Maui.Views;

namespace HOR0552.ViewModels;
public partial class SelectDiagnosisViewModel : ObservableObject
{

    [ObservableProperty]
    ObservableCollection<Diagnosis> allDiagnoses;

    [ObservableProperty]
    ObservableCollection<Diagnosis> selectedDiagnoses;

    [ObservableProperty]
    ObservableCollection<Diagnosis> filteredDiagnoses;

    List<string> selectedDiagnosesIDs;
    public SelectDiagnosisViewModel()
    {
        FilteredDiagnoses = new ObservableCollection<Diagnosis>();

        LoadAllDiagnoses();
        LoadSelectedDiagnoses();

        FilterDiagnoses("");
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
            AllDiagnoses = new ObservableCollection<Diagnosis>(diagnosesRoot.diagnoses);
        }
        else
        {
            AllDiagnoses = new ObservableCollection<Diagnosis>();
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
                SelectedDiagnoses = loadedDiagnoses;
            }
        }
        else
        {
            SelectedDiagnoses = new ObservableCollection<Diagnosis>();
        }

        selectedDiagnosesIDs = SelectedDiagnoses.Select(d => d.diagnosisId).ToList();
    }

    public void FilterDiagnoses(string searchText)
    {
        
        if (string.IsNullOrWhiteSpace(searchText))
        {
            FilteredDiagnoses.Clear();

            foreach (var diagnosis in AllDiagnoses)
            {
                if (!selectedDiagnosesIDs.Contains(diagnosis.diagnosisId))
                {
                    FilteredDiagnoses.Add(diagnosis);
                }
            }
        }
        else
        {
            var filteredList = AllDiagnoses
                .Where(d => d.name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .ToList();

            FilteredDiagnoses.Clear();

            foreach (var diagnosis in filteredList)
            {
                if (!selectedDiagnosesIDs.Contains(diagnosis.diagnosisId))
                {
                    FilteredDiagnoses.Add(diagnosis);
                }
            }
        }
    }
}

