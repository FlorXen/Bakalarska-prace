using System.Globalization;
using HOR0552.Models;

namespace HOR0552;

public class GetDeadlineInDaysFromDiagnosisConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Diagnosis diagnosis)
        {
            var currentStep = diagnosis.treatmentPlan.FirstOrDefault(step => step.step == diagnosis.currentStepNum);
            return currentStep?.deadlineInDays ?? 0;
        }
        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}