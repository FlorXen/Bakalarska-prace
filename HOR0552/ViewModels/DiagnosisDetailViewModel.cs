using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HOR0552.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOR0552.ViewModels;
[QueryProperty(nameof(SelectedDiagnosis), "SelectedDiagnosis")]
public partial class DiagnosisDetailViewModel : ObservableObject
{
    [ObservableProperty]
    Diagnosis selectedDiagnosis;
}
