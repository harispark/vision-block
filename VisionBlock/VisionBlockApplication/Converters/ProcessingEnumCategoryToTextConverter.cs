using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using VisionBlockApplication.ViewModels.Misc;

namespace VisionBlockApplication.Converters
{
    public class ProcessingEnumCategoryToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int category = (int)value;


            if (category == (int)ProcessingCategoryEnum.Color)
            {
                return "Color";
            }
            else if (category == (int)ProcessingCategoryEnum.GreyScale)
            {
                return "NDG";
            }
            else if (category == (int)ProcessingCategoryEnum.Binaire)
            {
                return "Binaire";
            }
            else if (category == (int)ProcessingCategoryEnum.All)
            {
                return "Ndg Binaire Color";
            }
            else if(category == (int)ProcessingCategoryEnum.Multiple)
            {
                return "Multiple";
            }
            else if (category == (int)ProcessingCategoryEnum.None)
            {
                return "None";
            }
            else if (category == (int)ProcessingCategoryEnum.RGBA)
            {
                return "Transparent";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
