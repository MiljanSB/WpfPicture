using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace WpfPicture
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string odabranaSlika = "";
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void PrikaziOsobe()
        {
            DataGrid1.Items.Clear();

            List<Osoba> listaOsoba = OsobaDal.VratiOsobe();
            if (listaOsoba!=null)
            {
                foreach (Osoba os in listaOsoba)
                {
                    DataGrid1.Items.Add(os);
                }
            }
        }

        private void Resetuj()
        {
            odabranaSlika = "";
            Image1.Source = null;
            TextBoxId.Clear();
            TextBoxIme.Clear();
            TextBoxPrezime.Clear();
            DataGrid1.SelectedIndex = -1;
            RadioMuski.IsChecked = true;
        }

        private bool Validacija()
        {
            if (string.IsNullOrWhiteSpace(TextBoxIme.Text))
            {
                MessageBox.Show("Unesite ime");
                TextBoxIme.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TextBoxPrezime.Text))
            {
                MessageBox.Show("Unesite prezime");
                TextBoxPrezime.Focus();
                return false;
            }
            return true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string folder = Putanja.VratiFolderSaSlikama();
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                MessageBox.Show("Kreiran folder za slike");
            }
            PrikaziOsobe();
        }

        private void ButtonResetuj_Click(object sender, RoutedEventArgs e)
        {
            Resetuj();
        }

        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid1.SelectedIndex >-1)
            {
                odabranaSlika = "";
                Osoba os = DataGrid1.SelectedItem as Osoba;

                TextBoxId.Text = os.OsobaId.ToString();
                TextBoxIme.Text = os.Ime;
                TextBoxPrezime.Text = os.Prezime;

                if (os.Pol == true)
                {
                    RadioZenski.IsChecked = true;
                }
                else
                {
                    RadioMuski.IsChecked = true;
                }

                string putanja = Putanja.VratiPutanjuSlike(os.Slika);

                Uri adresa = new Uri(putanja, UriKind.Absolute);
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = adresa;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                Image1.Source = bmp;
                
            }
        }

        private void ButtonOdaberi_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = @"C:\Slike";
            dlg.Filter = @"Slike|*.jpg;*.bmp;*.gif";

            if (dlg.ShowDialog() == true)
            {
                odabranaSlika = dlg.FileName;

                Uri adresa = new Uri(odabranaSlika, UriKind.Absolute);
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = adresa;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                Image1.Source = bmp;
            }
        }

        private void ButtonUbaci_Click(object sender, RoutedEventArgs e)
        {
            if (!Validacija())
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(odabranaSlika))
            {
                MessageBox.Show("Odaberi sliku");
                return;
            }

            string putanja = Putanja.KreirajOdrediste(odabranaSlika);

            Osoba os = new Osoba();
            os.Ime = TextBoxIme.Text;
            os.Prezime = TextBoxPrezime.Text;

            if (RadioMuski.IsChecked == true)
            {
                os.Pol = false;
            }
            else
            {
                os.Pol = true;
            }

            os.Slika = Path.GetFileName(putanja);

            int rez = OsobaDal.UbaciOsobu(os);

            if (rez == 0)
            {
                try
                {
                    File.Copy(odabranaSlika, putanja);
                }
                catch (Exception xcp)
                {
                    MessageBox.Show(xcp.Message);
                }

                PrikaziOsobe();
                DataGrid1.Focus();
                int indeks = DataGrid1.Items.Count - 1;
                DataGrid1.SelectedIndex = indeks;
                DataGrid1.ScrollIntoView(DataGrid1.Items[indeks]);
                odabranaSlika = "";
                MessageBox.Show("Podaci sacuvani");
            }
            else
            {
                MessageBox.Show("Greska pri cuvanju podataka");
            }
        }

        private void ButtonObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid1.SelectedIndex > -1)
            {

                Osoba os = DataGrid1.SelectedItem as Osoba;

                int rez = OsobaDal.ObrisiOsobu(os);

                if (rez == 0)
                {
                    PrikaziOsobe();
                    Resetuj();

                    string putanja = Putanja.VratiPutanjuSlike(os.Slika);

                    try
                    {
                        File.Delete(putanja);
                    }
                    catch (Exception xcp)
                    {
                        MessageBox.Show(xcp.Message);
                    }

                    MessageBox.Show("Podaci obrisani");
                }
            }
            else
            {
                MessageBox.Show("Odaberi osobu");
            }
        }

        private void ButtonPromeni_Click(object sender, RoutedEventArgs e)
        {
            int indeks = DataGrid1.SelectedIndex;
            if (DataGrid1.SelectedIndex < 0)
            {
                MessageBox.Show("Odaberi osobu");
                return;
            }

            if (!Validacija())
            {
                return;
            }

            Osoba os = DataGrid1.SelectedItem as Osoba;
            os.Ime = TextBoxIme.Text;
            os.Prezime = TextBoxPrezime.Text;

            if (RadioMuski.IsChecked==true)
            {
                os.Pol = false;
            }
            else
            {
                os.Pol = true;
            }

            string staraSlika = Putanja.VratiPutanjuSlike(os.Slika);

            string novaSlika = "";

            if (odabranaSlika !="")
            {
                //menjamo sliku 
                novaSlika = Putanja.KreirajOdrediste(odabranaSlika);
                os.Slika = Path.GetFileName(novaSlika);
            }

            int rez = OsobaDal.PromeniOsobu(os);

            if (rez == 0)
            {
                if (odabranaSlika != "")
                {
                    try
                    {
                        //slika promenjena
                        File.Copy(odabranaSlika, novaSlika);

                        //stara slika obrisana
                        File.Delete(staraSlika);
                    }
                    catch (Exception xcp)
                    {
                        MessageBox.Show(xcp.Message);
                        return;
                    }
                }
                PrikaziOsobe();
                DataGrid1.Focus();
                DataGrid1.SelectedIndex = indeks;
                DataGrid1.ScrollIntoView(DataGrid1.Items[indeks]);
                odabranaSlika = "";
                MessageBox.Show("Podaci promenjeni");
            }
            else
            {
                MessageBox.Show("Greska pri promeni podataka");
            }
        
        }
    }
}
