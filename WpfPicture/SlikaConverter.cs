using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WpfPicture
{
    class SlikaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ulaz = value?.ToString() ?? "";

            if (ulaz != "")
            {
                string putanja = Putanja.VratiPutanjuSlike(ulaz);
                Uri adresa = new Uri(putanja, UriKind.Absolute);
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = adresa;
                bmp.CacheOption = BitmapCacheOption.OnLoad; //zatvara stream
                bmp.EndInit();
                return bmp;
            }
            else
            {
                return null;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
