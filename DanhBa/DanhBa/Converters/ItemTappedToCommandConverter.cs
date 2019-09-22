using System;
using System.Globalization;
using Xamarin.Forms;

namespace DanhBa.Converters
{
    public class ItemTappedToCommandConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ItemTappedEventArgs e)
            {
                return e.Item;
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
