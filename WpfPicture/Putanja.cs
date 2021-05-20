using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WpfPicture
{
    static class Putanja
    {
        //Vraca putanju /bin/debug/Slike
        public static string VratiFolderSaSlikama()
        {
            string root = Directory.GetCurrentDirectory();
            string putanja = Path.Combine(root, "Slike");
            return putanja;
        }

        //Vraca putanju /bin/debug/Slike/...jpg
        public static string VratiPutanjuSlike(string slika)
        {
            string folder = VratiFolderSaSlikama();
            return Path.Combine(folder, slika);
        }

        //abs putanja fileslike na sistemu
        public static string KreirajOdrediste(string putanja)
        {
            string slika = Path.GetFileName(putanja);

            string imeBezEkstenzije = Path.GetFileNameWithoutExtension(slika);

            string ekstenzija = Path.GetExtension(putanja);
            
            string slikaOdrediste = VratiPutanjuSlike(slika);

            int brojac = 0;
            while (File.Exists(slikaOdrediste))
            {
                brojac++;
                slikaOdrediste = VratiPutanjuSlike(imeBezEkstenzije + brojac + ekstenzija);
            }

            return slikaOdrediste;
        }
    }
}
