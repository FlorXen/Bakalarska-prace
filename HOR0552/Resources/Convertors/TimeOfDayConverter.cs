using Microsoft.Maui.Platform;
using System.Globalization;

namespace HOR0552
{
    public class TimeOfDayConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeOfDay)
            {
                return timeOfDay == TimeSpan.Zero ? "Celý den" : timeOfDay.ToString(@"hh\:mm");
            }
            return value;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
