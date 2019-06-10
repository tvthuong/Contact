using System;
using System.Globalization;
using System.Net;
using Xamarin.Forms;

namespace DanhBa
{
    class StringUrlConverter:IValueConverter
    {
        private string validurl;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            validurl = (string)value;
            return validurl;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                WebClient wc = new WebClient();
                string HTMLSource = wc.DownloadString((string)value);
                validurl = (string)value;
            }
            catch (Exception) { }
            return validurl;
        }
    }
}
