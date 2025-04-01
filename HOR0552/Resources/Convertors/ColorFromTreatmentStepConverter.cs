using System.Globalization;
using HOR0552.Models;

namespace HOR0552;

public class ColorFromTreatmentStepConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var currentTheme = Application.Current.RequestedTheme;

        if (value is TreatmentStep step)
        {
            if (step.isCompleted)
            {
                return currentTheme == AppTheme.Dark ? "#398508" : "#59cf0c";
            } else if (step.stepDate != null)
            {
                return currentTheme == AppTheme.Dark ? "#0466cf" : "#0774e8";
            } else return currentTheme == AppTheme.Dark ? "#121111" : "#c9c9c9";
        }
        else
        {
            return currentTheme == AppTheme.Dark ? "#121111" : "#c9c9c9";
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}