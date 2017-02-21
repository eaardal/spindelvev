using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Spindelvev.Infrastructure;

namespace Spindelvev.Application.Converters
{
    class LogSeverityToBorderBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var severity = (LogSeverity)value;

                switch (severity)
                {
                    case LogSeverity.Verbose:
                        return new SolidColorBrush(Colors.Gray);
                    case LogSeverity.Info:
                        return new SolidColorBrush(Colors.DeepSkyBlue);
                    case LogSeverity.Warning:
                        return new SolidColorBrush(Colors.DarkOrange);
                    case LogSeverity.Error:
                        return new SolidColorBrush(Colors.DarkRed);
                    default:
                        return new SolidColorBrush(Colors.Black);
                }
            }
            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
