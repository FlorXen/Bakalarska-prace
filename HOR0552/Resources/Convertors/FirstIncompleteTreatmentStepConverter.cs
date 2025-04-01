using System.Globalization;
using HOR0552.Models;

namespace HOR0552;

public class FirstIncompleteTreatmentStepConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int? nextStepNum = null;

        if (value is List<TreatmentStep> treatmentSteps)
        {
            foreach (TreatmentStep step in treatmentSteps)
            {
                if (nextStepNum == null || nextStepNum == step.step)
                {
                    if (step.isCompleted)
                    {
                        if (step.nextStep != null)
                        {
                            nextStepNum = step.nextStep;
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    } else
                    {   
                        if(step.daysUntilDeadline > 0)
                        {
                            return step.procedure.name + "  Dní do termínu: " + step.daysUntilDeadline;
                        }
                        else
                        {
                            if(step.daysUntilDeadline < 0)
                                return step.procedure.name + "  Dní po termínu: " + step.daysUntilDeadline;
                            else
                                return step.procedure.name + "  Dnes je termín!";
                        }
                    }
                }
            }
            return "léčba dokončena";
        } else if (value is Diagnosis diagnosis)
        {
            foreach (TreatmentStep step in diagnosis.treatmentPlan)
            {
                if (nextStepNum == null || nextStepNum == step.step)
                {
                    if (step.isCompleted)
                    {
                        if (step.nextStep != null)
                        {
                            nextStepNum = step.nextStep;
                            continue;
                        }
                        else
                        {
                            return "false";
                        }
                    }
                    else return "true";
                }
            }
        }

        return "true";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}