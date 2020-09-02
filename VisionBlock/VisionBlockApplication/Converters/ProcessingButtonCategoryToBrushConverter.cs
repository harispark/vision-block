using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using VisionBlockApplication.Models;
using System.Windows.Media;
using VisionBlockApplication.ViewModels.Misc;

namespace VisionBlockApplication.Converters
{
    public class ProcessingButtonCategoryToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int category = (int)value;

            LinearGradientBrush _backgroundBrush = new LinearGradientBrush();

            if (category == (int)ProcessingCategoryEnum.Color)
            {
                _backgroundBrush.GradientStops.Add(new GradientStop(Colors.Blue, 0.0));
                _backgroundBrush.GradientStops.Add(new GradientStop(Colors.Orange, 0.25));
                _backgroundBrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.5));
                _backgroundBrush.GradientStops.Add(new GradientStop(Colors.LightGreen, 0.75));
                _backgroundBrush.GradientStops.Add(new GradientStop(Colors.Red, 1.0));
                return _backgroundBrush;
            }
            else if (category == (int)ProcessingCategoryEnum.GreyScale)
            {
                _backgroundBrush.GradientStops.Add(new GradientStop(Colors.Gray, 0.0));
                _backgroundBrush.GradientStops.Add(new GradientStop(Colors.LightGray, 1.0));
                return _backgroundBrush;
            }
            else if (category == (int)ProcessingCategoryEnum.Binaire)
            {
                return new SolidColorBrush(Colors.White);
            }
            return _backgroundBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
