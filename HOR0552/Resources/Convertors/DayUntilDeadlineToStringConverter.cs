using System.Globalization;
using HOR0552.Models;

namespace HOR0552;

public class DayUntilDeadlineToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        if (value is TreatmentStep step)
        {
            if(step.nextStep == null && step.nextSteps == null && step.isCompleted)
            {
                return "Léčba dokončena";
            }

            if (step.daysUntilDeadline > 0)
            {
                return "Dní do termínu: " + step.daysUntilDeadline;
            }
            else
            {
                if (step.daysUntilDeadline < 0)
                    return "Dní po termínu: " + step.daysUntilDeadline;
                else
                    return "Dnes je termín!";
            }
        }
        else
        {
            return "Chyba";
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}