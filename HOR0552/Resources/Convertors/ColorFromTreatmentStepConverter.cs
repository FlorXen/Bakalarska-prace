using System.Globalization;
using HOR0552.Models;

namespace HOR0552;

public class ColorFromTreatmentStepConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        if (value is TreatmentStep step)
        {
            if (step.isCompleted)
            {
                return "LightGreen";
            } else if (step.stepDate != null)
            {
                return "LightBlue";
            } else return "LightGray";
        }
        else
        {
            return "LightGray";
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}