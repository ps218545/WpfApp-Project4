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


        // Bestellingen

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


        // Units
        public string GetUnits(ICollection<Unit> units)
        {
            if (units == null)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van GetUnits");
            }
            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        SELECT id as unitName, name as unitNaam
                        FROM units
                        ORDER BY name
                    ";
                    MySqlDataReader reader = sql.ExecuteReader();

                    while (reader.Read())
                    {
                        Unit unit = new()
                        {
                            UnitId = (int)reader["unitName"],
                            UnitNaam = (string)reader["unitNaam"],

                        };
                        units.Add(unit);
                    }
                    methodResult = OK;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetUnits));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string GetUnit(int unitId, out Unit? unit)
        {
            unit = null;
            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        SELECT id as unitId, name as unitNaam
                        FROM ingredients
                        WHERE unitId = @unitId;
                        ";
                    sql.Parameters.AddWithValue("@unitId", unitId);
                    MySqlDataReader reader = sql.ExecuteReader();

                    while (reader.Read())
                    {
                        unit = new()
                        {
                            UnitId = (int)reader["unitId"],
                            UnitNaam = (string)reader["UnitNaam"],
                        };
                    }

                    methodResult = unit == null ? NOTFOUND : OK;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetUnit));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string CreateUnit(Unit unit)
        {
            if (unit == null || string.IsNullOrEmpty(unit.UnitNaam))
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van CreateUnit");
            }

            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                    INSERT INTO units 
                            (id, name) 
                    VALUES  (NULL, @name);
                    ";
                    sql.Parameters.AddWithValue("@name", unit.UnitNaam);

                    if (sql.ExecuteNonQuery() == 1)
                    {
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = $"Unit {unit.UnitNaam} kon niet toegevoegd worden.";
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(CreateUnit));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string UpdateUnit(int unitId, Unit unit)
        {
            if (unit == null || string.IsNullOrEmpty(unit.UnitNaam))
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van UpdateUnit");
            }

            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        UPDATE units
                        SET name = @name
                        WHERE id = @id;
                        ";
                    sql.Parameters.AddWithValue("@id", unitId);
                    sql.Parameters.AddWithValue("@name", unit.UnitNaam);

                    if (sql.ExecuteNonQuery() == 1)
                    {
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = $"Unit {unit.UnitNaam} kon niet gewijzigd worden.";
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(UpdateUnit));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string DeleteUnit(int unitId)
        {
            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        DELETE 
                        FROM units 
                        WHERE id = @id 
                    ";
                    sql.Parameters.AddWithValue("@id", unitId);
                    if (sql.ExecuteNonQuery() == 1)
                    {
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = $"Unit met id {unitId} kon niet verwijderd worden.";
                    }

                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(DeleteUnit));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }



        // Ingredients

        public string GetIngredients(ICollection<Ingredient> ingredients)
        {
            if (ingredients == null)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van GetIngredients");
            }

            string methodResult = UNKNOWN;


            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        SELECT i.id, i.name, i.price, i.unit_id, u.name as unitName
                        FROM ingredients i
                        INNER JOIN units u ON u.id = i.unit_id
                        ORDER BY i.name
                    ";
                    MySqlDataReader reader = sql.ExecuteReader();

                    while (reader.Read())
                    {
                        Ingredient ingredient = new()
                        {
                            IngredientId = (int)reader["id"],
                            Name = (string)reader["name"],
                            Price = (decimal)reader["price"],
                            UnitId = (int)reader["unit_id"],
                            Unit = new Unit()
                            {
                                UnitId = (int)reader["id"],
                                UnitNaam = (string)reader["unitName"],

                            }
                        };
                        ingredients.Add(ingredient);
                    }
                    methodResult = OK;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetIngredients));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string GetIngredient(int ingredientId, out Ingredient? ingredient)
        {
            ingredient = null;
            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        SELECT i.id, i.name, i.price, i.unit_id, u.name as unitName
                        FROM ingredients i
                        INNER JOIN units u ON u.id = i.unit_id
                        WHERE i.id = @ingredientId;
                        ";
                    sql.Parameters.AddWithValue("@ingredientId", ingredientId);
                    MySqlDataReader reader = sql.ExecuteReader();

                    while (reader.Read())
                    {
                        ingredient = new()
                        {
                            IngredientId = (int)reader["ingredientId"],
                            Name = (string)reader["name"],
                            Price = (decimal)reader["price"],
                            UnitId = (int)reader["unit_id"],
                            Unit = new Unit()
                            {
                                UnitId = (int)reader["id"],
                                UnitNaam = (string)reader["unitName"],

                            }
                        };
                    }

                    methodResult = ingredient == null ? NOTFOUND : OK;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetIngredient));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string CreateIngredient(Ingredient ingredient)
        {
            if (ingredient == null || string.IsNullOrEmpty(ingredient.Name)
                || ingredient.Price < 0 || ingredient.UnitId == 0)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van CreateIngredient");
            }

            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                    INSERT INTO ingredients 
                            (id,  name,  price,  unit_id) 
                    VALUES  (NULL,         @name, @price, @unitId);
                    ";
                    sql.Parameters.AddWithValue("@name", ingredient.Name);
                    sql.Parameters.AddWithValue("@price", ingredient.Price);
                    sql.Parameters.AddWithValue("@unitId", ingredient.UnitId);

                    if (sql.ExecuteNonQuery() == 1)
                    {
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = $"Ingrediënt {ingredient.Name} kon niet toegevoegd worden.";
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(CreateIngredient));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string UpdateIngredient(int ingredientId, Ingredient ingredient)
        {
            if (ingredient == null || string.IsNullOrEmpty(ingredient.Name)
                || ingredient.Price < 0 || ingredient.UnitId == 0)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van UpdateIngredient");
            }

            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        UPDATE ingredients
                        SET name = @name, 
                            price = @price,
                            unit_id = @unitId
                        WHERE id = @ingredientId;
                        ";
                    sql.Parameters.AddWithValue("@ingredientId", ingredientId);
                    sql.Parameters.AddWithValue("@name", ingredient.Name);
                    sql.Parameters.AddWithValue("@price", ingredient.Price);
                    sql.Parameters.AddWithValue("@unitId", ingredient.UnitId);

                    if (sql.ExecuteNonQuery() == 1)
                    {
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = $"Ingredient {ingredient.Name} kon niet gewijzigd worden.";
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(UpdateIngredient));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string DeleteIngredient(int ingredientId)
        {
            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        DELETE 
                        FROM ingredients 
                        WHERE id = @ingredientId 
                    ";
                    sql.Parameters.AddWithValue("@ingredientId", ingredientId);
                    if (sql.ExecuteNonQuery() == 1)
                    {
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = $"Ingredient met id {ingredientId} kon niet verwijderd worden.";
                    }

                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(DeleteIngredient));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

    }
}
