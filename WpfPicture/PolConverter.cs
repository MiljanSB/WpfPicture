using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
namespace WpfPicture
{
    class PolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? ulaz = null;

            if (value is bool)
            {
                ulaz = (bool)value;
            }

            string izlaz = "";
            if (ulaz == false)
            {
                izlaz = "Muski pol";
            }
            else if (ulaz == true)
            {
                izlaz = "Zenski pol";
            }
            else
            {
                izlaz = "Greska";
            }

            return izlaz;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
