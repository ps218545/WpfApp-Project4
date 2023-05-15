using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_Project4.Views;

namespace WpfApp_Project4.Models
{
    public class Project4db
    {
        #region Messages
        public static readonly string UNKNOWN = "Unknown";
        public static readonly string OK = "Ok";
        public static readonly string NOTFOUND = "not found";
        #endregion

        private readonly string connString = ConfigurationManager.ConnectionStrings["project4"].ConnectionString;



        public string GetBestellingen(ICollection<Bestelling> bestellingen)
        {
            if (bestellingen == null)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van GetBestellingen");
            }

            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        SELECT b.bestellingID, b.klantID, b.datum, p.id, p.first_name, p.last_name, p.postal_code, p.address, p.phone, p.personal_email
                        FROM bestellingen b
                        INNER JOIN people p ON p.id = b.klantId
                        ORDER BY b.bestellingID
                        ";
                    MySqlDataReader reader = sql.ExecuteReader();

                    while (reader.Read())
                    {
                        Bestelling bestelling = new Bestelling()
                        {
                            BestellingId = (int)reader["bestellingID"],
                            KlantId = (ulong)reader["klantID"],
                            Datum = (DateTime)reader["datum"],
                            KlantId2 = (ulong)reader["id"],
                            Klant = new Klant()
                            {
                                KlantId2 = (ulong)reader["id"],
                                VoorNaam = (string)reader["first_name"],
                                AchterNaam = (string)reader["last_name"],
                                PostCode = (string)reader["postal_code"],
                                Adres = (string)reader["address"],
                                Nummer = (string)reader["phone"],
                                Mail = (string)reader["personal_email"],

                            }
                        };
                        bestellingen.Add(bestelling);
                    }
                    methodResult = OK;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetBestellingen));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }



        public string GetBestelRegelsByBestelling(int bestellingId, ICollection<Bestelregel> bestelRegels)
        {
            if (bestelRegels == null)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van GetBestelRegelsByBestelling");
            }

            string methodResult = UNKNOWN;


            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                            SELECT br.bestelregelID, br.bestellingID, br.productId,
                                   p.id as 'ProductId', p.name
                            FROM bestelregels br
                            INNER JOIN products p ON p.id = br.productId
                            WHERE br.bestellingID = @bestellingID
                        ";
                    sql.Parameters.AddWithValue("@bestellingID", bestellingId);
                    MySqlDataReader reader = sql.ExecuteReader();

                    while (reader.Read())
                    {
                        Bestelregel bestelRegel = new()
                        {
                            BestelRegelId = (int)reader["bestelregelID"],
                            BestellingId = (int)reader["bestellingID"],
                            // afmeting
                            ProductId = (ulong)reader["ProductId"],
                            Product = new()
                            {
                                ProductId = (ulong)reader["ProductId"],
                                ProductName = (string)reader["name"],
                                // aantal
                            }
                        };
                        bestelRegels.Add(bestelRegel);
                    }
                    methodResult = OK;

                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetBestelRegelsByBestelling));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

    }
}
