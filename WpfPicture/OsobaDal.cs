using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace WpfPicture
{
    static class OsobaDal
    {
        public static List<Osoba> VratiOsobe()
        {
            List<Osoba> listaOsoba = new List<Osoba>();

            using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnBazaOsoba))
            {
                using (SqlCommand komanda = new SqlCommand("SELECT * FROM Osoba",konekcija))
                {
                    konekcija.Open();
                    using (SqlDataReader dr = komanda.ExecuteReader())
                    {
                        try
                        {
                            while (dr.Read())
                            {
                                Osoba o = new Osoba
                                {
                                    OsobaId = dr.GetInt32(0),
                                    Ime = dr.GetString(1),
                                    Prezime = dr.GetString(2),
                                    Pol = dr.GetBoolean(3),
                                    Slika = dr.GetString(4)
                                };
                                listaOsoba.Add(o);
                            }
                            return listaOsoba;
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public static int UbaciOsobu(Osoba o)
        {
            string upit = "INSERT INTO Osoba VALUES(@Ime,@Prezime,@Pol,@Slika)";
            using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnBazaOsoba))
            {
                using (SqlCommand komanda = new SqlCommand(upit, konekcija))
                {
                    komanda.Parameters.AddWithValue("@Ime", o.Ime);
                    komanda.Parameters.AddWithValue("@Prezime", o.Prezime);
                    komanda.Parameters.AddWithValue("@Pol", o.Pol);
                    komanda.Parameters.AddWithValue("@Slika", o.Slika);
                    try
                    {
                        konekcija.Open();
                        komanda.ExecuteNonQuery();

                        return 0;
                    }
                    catch (Exception)
                    {
                        return -1;
                    }

                }
            }
        }

        public static int PromeniOsobu(Osoba o)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UPDATE Osoba");
            sb.AppendLine("SET Ime = @Ime,Prezime = @Prezime,Pol = @Pol,Slika = @Slika");
            sb.AppendLine("WHERE OsobaId = @OsobaId");

            using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnBazaOsoba))
            {
                using (SqlCommand komanda = new SqlCommand(sb.ToString(),konekcija))
                {

                    komanda.Parameters.AddWithValue("@Ime", o.Ime);
                    komanda.Parameters.AddWithValue("@Prezime", o.Prezime);
                    komanda.Parameters.AddWithValue("@Pol", o.Pol);
                    komanda.Parameters.AddWithValue("@Slika", o.Slika);
                    komanda.Parameters.AddWithValue("@OsobaId", o.OsobaId);

                    try
                    {
                        konekcija.Open();
                        komanda.ExecuteNonQuery();

                        return 0;
                    }
                    catch (Exception)
                    {
                        return -1;
                    }
                }
            }
        }

        public static int ObrisiOsobu(Osoba o)
        {
            using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnBazaOsoba))
            {
                using (SqlCommand komanda = new SqlCommand("DELETE FROM Osoba WHERE OsobaId=@OsobaId",konekcija))
                {
                    komanda.Parameters.AddWithValue("@OsobaId", o.OsobaId);
                    try
                    {
                        konekcija.Open();
                        komanda.ExecuteNonQuery();
                        return 0;
                    }
                    catch (Exception)
                    {
                        return -1;
                    }
                }
            }
        }
    }
}
